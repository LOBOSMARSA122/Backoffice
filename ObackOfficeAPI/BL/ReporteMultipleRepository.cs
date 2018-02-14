using BE.Comun;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Web.Helpers;

namespace BL
{
    public class ReporteMultipleRepository
    {
        private DatabaseContext ctx = new DatabaseContext();

        public BandejaReporteMultiple BandejaReporteMultiple(BandejaReporteMultiple data)
        {
            try
            {
                int NoEsEliminado = (int)Enumeradores.EsEliminado.No;
                int TipoDocumentoGroupId = (int)Enumeradores.GrupoParametros.TipoDocumentos;
                int SedeGroupId = (int)Enumeradores.GrupoParametros.Sedes;
                int ConsidionGroupId = (int)Enumeradores.GrupoParametros.Condición;
                int AsistenciaGroupId = (int)Enumeradores.GrupoParametros.Asistencia;
                string NombreEmpleado = string.IsNullOrWhiteSpace(data.NombreEmpleado) ? null : data.NombreEmpleado;
                string DNIEmpleado = string.IsNullOrWhiteSpace(data.DNIEmpleado) ? null : data.DNIEmpleado;
                int skip = (data.Index - 1) * data.Take;

                var return_data = (from a in ctx.CursosProgramados
                                   join b in ctx.Eventos on a.EventoId equals b.EventoId
                                   join c in ctx.Cursos on a.CursoId equals c.CursoId
                                   join e in ctx.SalonProgramados on a.CursoProgramadoId equals e.CursoProgramadoId
                                   join g in ctx.EmpleadoCursos on e.SalonProgramadoId equals g.SalonProgramadoId
                                   join d in ctx.EmpleadoAsistencias on g.EmpleadoCursoId equals d.EmpleadoCursoId
                                   join i in ctx.Empleados on g.EmpleadoId equals i.EmpleadoId
                                   join j in ctx.Personas on i.PersonaId equals j.PersonaId
                                   join k in ctx.Parametros on new { a = j.TipoDocumentoId, b = TipoDocumentoGroupId } equals new { a = k.ParametroId, b = k.GrupoId }
                                   join l in ctx.Parametros on new { a = b.SedeId, b = SedeGroupId } equals new { a = l.ParametroId, b = l.GrupoId }
                                   join m in ctx.Parametros on new { a = g.CondicionId, b = ConsidionGroupId } equals new { a = m.ParametroId, b = m.GrupoId }
                                   join o in ctx.Parametros on new { a = d.Asistio, b = AsistenciaGroupId } equals new { a = o.ParametroId, b = o.GrupoId }
                                   where
                                   (data.SedeId == -1 || data.SedeId == b.SedeId) &&
                                   (data.EventoId == -1 || data.EventoId == b.EventoId) &&
                                   (data.CursoId == -1 || data.CursoId == a.CursoId) &&
                                   (NombreEmpleado == null || (j.Nombres + " " + j.ApellidoPaterno + " " + j.ApellidoMaterno).Contains(NombreEmpleado)) &&
                                   (DNIEmpleado == null || j.NroDocumento.Contains(DNIEmpleado)) &&
                                   a.EsEliminado == NoEsEliminado
                                   group new { b, c, d, g, j, k, l, m, o } by g.EmpleadoCursoId into grp
                                   select new ReporteMultipleList
                                   {
                                       EmpleadoCursoId = grp.Key,
                                       PersonaId = grp.FirstOrDefault().j.PersonaId,
                                       Nombre = grp.FirstOrDefault().j.Nombres + " " + grp.FirstOrDefault().j.ApellidoPaterno + " " + grp.FirstOrDefault().j.ApellidoMaterno,
                                       TipoDocumento = grp.FirstOrDefault().k.Valor1,
                                       NroDocumento = grp.FirstOrDefault().j.NroDocumento,
                                       Sede = grp.FirstOrDefault().l.Valor1,
                                       Evento = grp.FirstOrDefault().b.Nombre,
                                       Curso = grp.FirstOrDefault().c.NombreCurso,
                                       Nota = grp.FirstOrDefault().g.Nota,
                                       NotaTaller = grp.FirstOrDefault().g.NotaTaller,
                                       Condicion = grp.FirstOrDefault().m.Valor1,
                                       Asistencia = grp.GroupBy(x => x.d.EmpleadoAsistenciaId).Select(x => x.FirstOrDefault().o.Valor1).ToList()
                                   }).ToList();

                int TotalRegistros = return_data.Count;

                if (data.Take > 0)
                    return_data = return_data.Skip(skip).Take(data.Take).ToList();

                int[] X = return_data.Select(x => x.Asistencia.Count).ToArray();
                int? maximo = (from a in X orderby a descending select a).FirstOrDefault();

                data.Lista = return_data;
                data.MaximasAsistencias = maximo.HasValue ? maximo.Value : 1;
                data.TotalRegistros = TotalRegistros;

                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public byte[] ChartAsistencia(BandejaReporteMultiple data)
        {
            string NombreEmpleado = string.IsNullOrWhiteSpace(data.NombreEmpleado) ? null : data.NombreEmpleado;
            string DNIEmpleado = string.IsNullOrWhiteSpace(data.DNIEmpleado) ? null : data.DNIEmpleado;
            int NoEsEliminado = (int)Enumeradores.EsEliminado.No;

            int AsistieronID = (int)Enumeradores.Asistencia.Asistio;
            int FaltaronID = (int)Enumeradores.Asistencia.Falto;
            int PorIniciarID = (int)Enumeradores.Asistencia.PorIniciar;


            var listado = (from a in ctx.EmpleadoCursos
                           join b in ctx.SalonProgramados on a.SalonProgramadoId equals b.SalonProgramadoId
                           join c in ctx.CursosProgramados on b.CursoProgramadoId equals c.CursoProgramadoId
                           join d in ctx.Cursos on c.CursoId equals d.CursoId
                           join e in ctx.EmpleadoAsistencias on a.EmpleadoCursoId equals e.EmpleadoCursoId
                           join f in ctx.Empleados on a.EmpleadoId equals f.EmpleadoId
                           join g in ctx.Eventos on c.EventoId equals g.EventoId
                           join h in ctx.Personas on f.PersonaId equals h.PersonaId
                           where 
                           (data.CursoId == -1 || d.CursoId == data.CursoId) &&
                           (data.EventoId == -1 || c.EventoId == data.EventoId) &&
                           (data.SedeId == -1 || g.SedeId == data.SedeId) &&
                           (NombreEmpleado == null || (h.Nombres + " " + h.ApellidoPaterno + " " + h.ApellidoMaterno).Contains(NombreEmpleado)) &&
                           (DNIEmpleado == null || h.NroDocumento.Contains(DNIEmpleado)) &&
                           a.EsEliminado == NoEsEliminado
                           group new { d, e } by d.CursoId into grp
                           select new
                           {
                               Curso = grp.FirstOrDefault().d.NombreCurso,
                               Asistieron = grp.GroupBy(x => x.e.EmpleadoAsistenciaId).Where(x => x.FirstOrDefault().e.Asistio == AsistieronID).Count(),
                               Faltaron = grp.GroupBy(x => x.e.EmpleadoAsistenciaId).Where(x => x.FirstOrDefault().e.Asistio == FaltaronID).Count(),
                               PorIniciar = grp.GroupBy(x => x.e.EmpleadoAsistenciaId).Where(x => x.FirstOrDefault().e.Asistio == PorIniciarID).Count()
                           }).ToList();

            var Chart = new Chart(800, 600);
            Chart.AddTitle("Asistencias Por Cursos");
            Chart.AddSeries(name: "Asistieron",
            xValue: listado.Select(x => x.Curso).ToArray(),
            yValues: listado.Select(x => x.Asistieron).ToArray());

            Chart.AddSeries(name: "Faltaron",
            xValue: listado.Select(x => x.Curso).ToArray(),
            yValues: listado.Select(x => x.Faltaron).ToArray());

            Chart.AddSeries(name: "Por Iniciar",
            xValue: listado.Select(x => x.Curso).ToArray(),
            yValues: listado.Select(x => x.PorIniciar).ToArray());


            Chart.AddLegend("Leyenda");

            byte[] bytes = Chart.GetBytes();


            return bytes;
        }

        public byte[] ChartAprobados(BandejaReporteMultiple data)
        {
            string NombreEmpleado = string.IsNullOrWhiteSpace(data.NombreEmpleado) ? null : data.NombreEmpleado;
            string DNIEmpleado = string.IsNullOrWhiteSpace(data.DNIEmpleado) ? null : data.DNIEmpleado;
            int NoEsEliminado = (int)Enumeradores.EsEliminado.No;

            int AprobaronID = (int)Enumeradores.Condicion.Aprobado;
            int DesaprobaronID = (int)Enumeradores.Condicion.Desaprobado;
            int PorIniciarID = (int)Enumeradores.Condicion.PorIniciar;


            var listado = (from a in ctx.EmpleadoCursos
                           join b in ctx.SalonProgramados on a.SalonProgramadoId equals b.SalonProgramadoId
                           join c in ctx.CursosProgramados on b.CursoProgramadoId equals c.CursoProgramadoId
                           join d in ctx.Cursos on c.CursoId equals d.CursoId
                           join e in ctx.Empleados on a.EmpleadoId equals e.EmpleadoId
                           join f in ctx.Eventos on c.EventoId equals f.EventoId
                           join g in ctx.Personas on e.PersonaId equals g.PersonaId
                           where
                           (data.CursoId == -1 || d.CursoId == data.CursoId) &&
                           (data.EventoId == -1 || c.EventoId == data.EventoId) &&
                           (data.SedeId == -1 || f.SedeId == data.SedeId) &&
                           (NombreEmpleado == null || (g.Nombres + " " + g.ApellidoPaterno + " " + g.ApellidoMaterno).Contains(NombreEmpleado)) &&
                           (DNIEmpleado == null || g.NroDocumento.Contains(DNIEmpleado)) &&
                           a.EsEliminado == NoEsEliminado
                           group new { a, d } by d.CursoId into grp
                           select new
                           {
                               Curso = grp.FirstOrDefault().d.NombreCurso,
                               Aprobaron = grp.Where(X => X.a.CondicionId == AprobaronID).Count(),
                               Desaprobaron = grp.Where(X => X.a.CondicionId == DesaprobaronID).Count(),
                               PorIniciar = grp.Where(X => X.a.CondicionId == PorIniciarID).Count()
                           }).ToList();

            var Chart = new Chart(800, 600);
            Chart.AddTitle("Aprobados Por Cursos");
            Chart.AddSeries(name: "Aprobaron",
            xValue: listado.Select(x => x.Curso).ToArray(),
            yValues: listado.Select(x => x.Aprobaron).ToArray());

            Chart.AddSeries(name: "Desaprobaron",
            xValue: listado.Select(x => x.Curso).ToArray(),
            yValues: listado.Select(x => x.Desaprobaron).ToArray());

            Chart.AddSeries(name: "Por Iniciar",
            xValue: listado.Select(x => x.Curso).ToArray(),
            yValues: listado.Select(x => x.PorIniciar).ToArray());

            Chart.AddLegend("Leyenda");

            byte[] bytes = Chart.GetBytes();


            return bytes;
        }

        public byte[] ChartPromedio(BandejaReporteMultiple data)
        {
            string NombreEmpleado = string.IsNullOrWhiteSpace(data.NombreEmpleado) ? null : data.NombreEmpleado;
            string DNIEmpleado = string.IsNullOrWhiteSpace(data.DNIEmpleado) ? null : data.DNIEmpleado;
            int NoEsEliminado = (int)Enumeradores.EsEliminado.No;

            var listado = (from a in ctx.EmpleadoCursos
                           join b in ctx.SalonProgramados on a.SalonProgramadoId equals b.SalonProgramadoId
                           join c in ctx.CursosProgramados on b.CursoProgramadoId equals c.CursoProgramadoId
                           join d in ctx.Cursos on c.CursoId equals d.CursoId
                           join e in ctx.Empleados on a.EmpleadoId equals e.EmpleadoId
                           join f in ctx.Eventos on c.EventoId equals f.EventoId
                           join g in ctx.Personas on e.PersonaId equals g.PersonaId
                           where
                           (data.CursoId == -1 || d.CursoId == data.CursoId) &&
                           (data.EventoId == -1 || c.EventoId == data.EventoId) &&
                           (data.SedeId == -1 || f.SedeId == data.SedeId) &&
                           (NombreEmpleado == null || (g.Nombres + " " + g.ApellidoPaterno + " " + g.ApellidoMaterno).Contains(NombreEmpleado)) &&
                           (DNIEmpleado == null || g.NroDocumento.Contains(DNIEmpleado)) &&
                           a.EsEliminado == NoEsEliminado
                           group new { a, d } by d.CursoId into grp
                           select new
                           {
                               Curso = grp.FirstOrDefault().d.NombreCurso,
                               Promedio = grp.Select(x => x.a.Nota).Sum() / (decimal)grp.Select(x => x.a.Nota).Count()
                           }).ToList();

            var Chart = new Chart(800, 600);
            Chart.AddTitle("Promedio Por Cursos");
            Chart.AddSeries(name: "Promedio",
            xValue: listado.Select(x => x.Curso).ToArray(),
            yValues: listado.Select(x => x.Promedio).ToArray());


            byte[] bytes = Chart.GetBytes();

            return bytes;
        }

        public MemoryStream BandejaReporteMultipleExcel(BandejaReporteMultiple data, FileStream TemplateFile)
        {
            try
            {
                bool templateDeleted = false;

                IWorkbook TemplateBook = new XSSFWorkbook(TemplateFile);
                ISheet TemplateSheet = TemplateBook.GetSheet(TemplateBook.GetSheetName(0));

                ICell CeldaTitulo = TemplateSheet.GetRow(99).GetCell(0);
                ICell CeldaNormal = TemplateSheet.GetRow(100).GetCell(0);

                string[] chartList = data.Charts == null ? new List<string>().ToArray() : data.Charts;

                data = BandejaReporteMultiple(data);

                int index = 0;
                int indexcell = 0;

                IRow TemplateRow = TemplateSheet.CreateRow(index);
                index++;
                #region TITULOS
                ICell TemplateCell = TemplateRow.CreateCell(indexcell);
                TemplateCell.CellStyle = CeldaTitulo.CellStyle;
                TemplateCell.SetCellValue("Alumno");
                indexcell++;

                TemplateCell = TemplateRow.CreateCell(indexcell);
                TemplateCell.CellStyle = CeldaTitulo.CellStyle;
                TemplateCell.SetCellValue("Tipo Documento");
                indexcell++;

                TemplateCell = TemplateRow.CreateCell(indexcell);
                TemplateCell.CellStyle = CeldaTitulo.CellStyle;
                TemplateCell.SetCellValue("Nro Documento");
                indexcell++;

                TemplateCell = TemplateRow.CreateCell(indexcell);
                TemplateCell.CellStyle = CeldaTitulo.CellStyle;
                TemplateCell.SetCellValue("Sede");
                indexcell++;

                TemplateCell = TemplateRow.CreateCell(indexcell);
                TemplateCell.CellStyle = CeldaTitulo.CellStyle;
                TemplateCell.SetCellValue("Evento");
                indexcell++;

                TemplateCell = TemplateRow.CreateCell(indexcell);
                TemplateCell.CellStyle = CeldaTitulo.CellStyle;
                TemplateCell.SetCellValue("Curso");
                indexcell++;

                for (int i = 0; i < data.MaximasAsistencias; i++)
                {
                    TemplateCell = TemplateRow.CreateCell(indexcell);
                    TemplateCell.CellStyle = CeldaTitulo.CellStyle;
                    TemplateCell.SetCellValue("A " + i);
                    indexcell++;
                }

                TemplateCell = TemplateRow.CreateCell(indexcell);
                TemplateCell.CellStyle = CeldaTitulo.CellStyle;
                TemplateCell.SetCellValue("Nota");
                indexcell++;

                TemplateCell = TemplateRow.CreateCell(indexcell);
                TemplateCell.CellStyle = CeldaTitulo.CellStyle;
                TemplateCell.SetCellValue("Taller");
                indexcell++;

                TemplateCell = TemplateRow.CreateCell(indexcell);
                TemplateCell.CellStyle = CeldaTitulo.CellStyle;
                TemplateCell.SetCellValue("Condición");
                indexcell++;
                #endregion
                foreach (var Alumno in data.Lista)
                {
                    indexcell = 0;

                    TemplateRow = TemplateSheet.CreateRow(index);
                    index++;
                    #region CAMPOS
                    TemplateCell = TemplateRow.CreateCell(indexcell);
                    TemplateCell.CellStyle = CeldaNormal.CellStyle;
                    TemplateCell.SetCellValue(Alumno.Nombre);
                    indexcell++;

                    TemplateCell = TemplateRow.CreateCell(indexcell);
                    TemplateCell.CellStyle = CeldaNormal.CellStyle;
                    TemplateCell.SetCellValue(Alumno.TipoDocumento);
                    indexcell++;

                    TemplateCell = TemplateRow.CreateCell(indexcell);
                    TemplateCell.CellStyle = CeldaNormal.CellStyle;
                    TemplateCell.SetCellValue(Alumno.NroDocumento);
                    indexcell++;

                    TemplateCell = TemplateRow.CreateCell(indexcell);
                    TemplateCell.CellStyle = CeldaNormal.CellStyle;
                    TemplateCell.SetCellValue(Alumno.Sede);
                    indexcell++;

                    TemplateCell = TemplateRow.CreateCell(indexcell);
                    TemplateCell.CellStyle = CeldaNormal.CellStyle;
                    TemplateCell.SetCellValue(Alumno.Evento);
                    indexcell++;

                    TemplateCell = TemplateRow.CreateCell(indexcell);
                    TemplateCell.CellStyle = CeldaNormal.CellStyle;
                    TemplateCell.SetCellValue(Alumno.Curso);
                    indexcell++;

                    for (int i = 0; i < data.MaximasAsistencias; i++)
                    {
                        TemplateCell = TemplateRow.CreateCell(indexcell);
                        TemplateCell.CellStyle = CeldaNormal.CellStyle;
                        TemplateCell.SetCellValue(Alumno.Asistencia.Count <= i ? "N/A" : Alumno.Asistencia[i]);
                        indexcell++;
                    }

                    TemplateCell = TemplateRow.CreateCell(indexcell);
                    TemplateCell.CellStyle = CeldaNormal.CellStyle;
                    TemplateCell.SetCellValue(Alumno.Nota.ToString());
                    indexcell++;

                    TemplateCell = TemplateRow.CreateCell(indexcell);
                    TemplateCell.CellStyle = CeldaNormal.CellStyle;
                    TemplateCell.SetCellValue(Alumno.NotaTaller);
                    indexcell++;

                    TemplateCell = TemplateRow.CreateCell(indexcell);
                    TemplateCell.CellStyle = CeldaNormal.CellStyle;
                    TemplateCell.SetCellValue(Alumno.Condicion);
                    #endregion

                    if (!templateDeleted)
                    {
                        TemplateSheet.RemoveRow(TemplateSheet.GetRow(104));
                        TemplateSheet.RemoveRow(TemplateSheet.GetRow(103));
                        TemplateSheet.RemoveRow(TemplateSheet.GetRow(102));
                        TemplateSheet.RemoveRow(TemplateSheet.GetRow(101));
                        TemplateSheet.RemoveRow(TemplateSheet.GetRow(100));
                        TemplateSheet.RemoveRow(TemplateSheet.GetRow(99));
                        templateDeleted = true;
                    }
                }

                for (int i = 0; i <= indexcell; i++)
                {
                    TemplateSheet.AutoSizeColumn(i);
                }

                foreach (var chart in chartList)
                {
                    byte[] Image = null;
                    switch (chart)
                    {
                        case "Asistencia":
                            {
                                Image = ChartAsistencia(data);
                                break;
                            }
                        case "Aprobados":
                            {
                                Image = ChartAprobados(data);
                                break;
                            }
                        case "Promedios":
                            {
                                Image = ChartPromedio(data);
                                break;
                            }
                    }

                    if (Image != null)
                    {
                        int pictureIndex = TemplateBook.AddPicture(Image, PictureType.PNG);
                        ICreationHelper helper = TemplateBook.GetCreationHelper();
                        IDrawing drawing = TemplateSheet.CreateDrawingPatriarch();

                        IClientAnchor anchor = helper.CreateClientAnchor();
                        anchor.Col1 = 0;//0 index based column
                        anchor.Row1 = index;//0 index based row
                        IPicture picture = drawing.CreatePicture(anchor, pictureIndex);
                        picture.Resize(); //will reset client anchor

                        index = index + 30;
                    }
                }

                MemoryStream ms = new MemoryStream();
                using (MemoryStream tempStream = new MemoryStream())
                {
                    TemplateBook.Write(tempStream);
                    var byteArray = tempStream.ToArray();
                    ms.Write(byteArray, 0, byteArray.Length);
                }

                return ms;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
