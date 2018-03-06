using BE.Comun;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Web.Helpers;
using System.Globalization;

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
                string Categoria = string.IsNullOrWhiteSpace(data.Categoria) ? null : data.Categoria;
                string Area = string.IsNullOrWhiteSpace(data.Area) ? null : data.Area;
                int EmpresaId = 0;
                if (!string.IsNullOrWhiteSpace(data.Empresa))
                {
                    EmpresaId = (from a in ctx.Empresas where a.RazonSocial == data.Empresa select a.EmpresaId).FirstOrDefault();
                }
                int skip = (data.Index - 1) * data.Take;

                bool validfi = DateTime.TryParseExact(data.FechaInicio,"yyyy/MM/dd",CultureInfo.InvariantCulture,DateTimeStyles.None, out DateTime fi);
                bool validff = DateTime.TryParseExact(data.FechaFin, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ff);

                var query = (from a in ctx.CursosProgramados
                                   join b in ctx.Eventos on a.EventoId equals b.EventoId
                                   join c in ctx.Cursos on a.CursoId equals c.CursoId
                                   join e in ctx.SalonProgramados on a.CursoProgramadoId equals e.CursoProgramadoId
                                   join g in ctx.EmpleadoCursos on e.SalonProgramadoId equals g.SalonProgramadoId
                                   join f in ctx.Empresas on g.EmpresaId equals f.EmpresaId
                                   join d in ctx.EmpleadoAsistencias on g.EmpleadoCursoId equals d.EmpleadoCursoId
                                   join i in ctx.Empleados on g.EmpleadoId equals i.EmpleadoId
                                   join j in ctx.Personas on i.PersonaId equals j.PersonaId
                                   join k in ctx.Parametros on new { a = j.TipoDocumentoId, b = TipoDocumentoGroupId } equals new { a = k.ParametroId, b = k.GrupoId }
                                   join l in ctx.Parametros on new { a = b.SedeId, b = SedeGroupId } equals new { a = l.ParametroId, b = l.GrupoId }
                                   join m in ctx.Parametros on new { a = g.CondicionId, b = ConsidionGroupId } equals new { a = m.ParametroId, b = m.GrupoId }
                                   join n in ctx.CursosProgramados on  e.CursoProgramadoId equals n.CursoProgramadoId
                                   where
                                   (data.SedeId == -1 || data.SedeId == b.SedeId) &&
                                   (data.EventoId == -1 || data.EventoId == b.EventoId) &&
                                   (data.CursoId == -1 || data.CursoId == a.CursoId) &&
                                   (NombreEmpleado == null || (j.Nombres + " " + j.ApellidoPaterno + " " + j.ApellidoMaterno).Contains(NombreEmpleado) || j.NroDocumento.Contains(NombreEmpleado)) &&
                                   (data.Asistencia == -1 || data.Asistencia == d.Asistio) &&
                                   (data.Condicion == -1 || data.Condicion == g.CondicionId) &&
                                   (data.Ranking == null || g.NotaFinal >= data.Ranking) &&
                                   (!validfi || n.FechaInicio >= fi) && 
                                   (!validff || n.FechaInicio <= ff) &&
                                   (Area == null || i.Area == Area) &&
                                   (Categoria == null || i.Cargo == Categoria) &&
                                   (EmpresaId == 0 || g.EmpresaId == EmpresaId) &&
                                   (data.CapacitadorId == -1 || e.CapacitadorId == data.CapacitadorId) &&
                                   a.EsEliminado == NoEsEliminado
                                   select new
                                   {
                                       g.EmpleadoCursoId,
                                       j.PersonaId,
                                       Nombre = j.Nombres + " " + j.ApellidoPaterno + " " + j.ApellidoMaterno,
                                       TipoDocumento = k.Valor1,
                                       j.NroDocumento,
                                       Sede = l.Valor1,
                                       Evento = b.Nombre,
                                       Curso = c.NombreCurso,
                                       Empresa = f.RazonSocial,
                                       g.Nota,
                                       g.NotaTaller,
                                       Condicion = m.Valor1,
                                       Asistencia = d.Asistio,
                                       AsistenciaID = d.EmpleadoAsistenciaId,
                                       g.NotaFinal,
                                       g.TallerValor
                                   }).ToList();

                var parametroAsistencia = (from a in ctx.Parametros where a.GrupoId == AsistenciaGroupId select a).ToList();

                var return_data = (from a in query
                                   group a by a.EmpleadoCursoId into grp
                                   select new ReporteMultipleList()
                                   {
                                       EmpleadoCursoId = grp.Key,
                                       PersonaId = grp.FirstOrDefault().PersonaId,
                                       Nombre = grp.FirstOrDefault().Nombre,
                                       TipoDocumento = grp.FirstOrDefault().TipoDocumento,
                                       NroDocumento = grp.FirstOrDefault().NroDocumento,
                                       Sede = grp.FirstOrDefault().Sede,
                                       Evento = grp.FirstOrDefault().Evento,
                                       Curso = grp.FirstOrDefault().Curso,
                                       Empresa = grp.FirstOrDefault().Empresa,
                                       Nota = grp.FirstOrDefault().Nota,
                                       NotaTaller = grp.FirstOrDefault().NotaTaller,
                                       Condicion = grp.FirstOrDefault().Condicion,
                                       Asistencia = grp.GroupBy(x => x.AsistenciaID).Select( y => y.FirstOrDefault().Asistencia.HasValue ? parametroAsistencia.Where(z => z.ParametroId == y.FirstOrDefault().Asistencia.Value).FirstOrDefault().Valor1 : "Por Iniciar").ToList(),
                                       NotaFinal = grp.FirstOrDefault().NotaFinal,
                                       TallerValor = grp.FirstOrDefault().TallerValor
                                   }).ToList();

                data.TotalRegistros = return_data.Count;

                if (data.Take > 0)
                    return_data = return_data.Skip(skip).Take(data.Take).ToList();

                int[] X = return_data.Select(x => x.Asistencia.Count).ToArray();
                int? maximo = (from a in X orderby a descending select a).FirstOrDefault();

                data.Lista = return_data;
                data.MaximasAsistencias = maximo.HasValue ? maximo.Value : 1;

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
            string Categoria = string.IsNullOrWhiteSpace(data.Categoria) ? null : data.Categoria;
            string Area = string.IsNullOrWhiteSpace(data.Area) ? null : data.Area;
            int EmpresaId = 0;
            if (!string.IsNullOrWhiteSpace(data.Empresa))
            {
                EmpresaId = (from a in ctx.Empresas where a.RazonSocial == data.Empresa select a.EmpresaId).FirstOrDefault();
            }
            int NoEsEliminado = (int)Enumeradores.EsEliminado.No;

            int AsistieronID = (int)Enumeradores.Asistencia.Asistio;
            int FaltaronID = (int)Enumeradores.Asistencia.Falto;

            bool validfi = DateTime.TryParseExact(data.FechaInicio, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fi);
            bool validff = DateTime.TryParseExact(data.FechaFin, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ff);

            var listado_temp = (from a in ctx.EmpleadoCursos
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
                           (NombreEmpleado == null || (h.Nombres + " " + h.ApellidoPaterno + " " + h.ApellidoMaterno).Contains(NombreEmpleado) || h.NroDocumento.Contains(NombreEmpleado)) &&
                           (data.Condicion == -1 || data.Condicion == a.CondicionId) &&
                           (data.Asistencia == -1 || data.Asistencia == e.Asistio) &&
                           (Area == null ^ f.Area == Area) &&
                           (Categoria == null ^ f.Cargo == Categoria) &&
                           (EmpresaId == 0 || a.EmpresaId == EmpresaId) &&
                           (data.CapacitadorId == -1 || b.CapacitadorId == data.CapacitadorId) &&
                           (!validfi || c.FechaInicio >= fi) &&
                            (!validff || c.FechaInicio <= ff) &&
                           a.EsEliminado == NoEsEliminado
                           select new
                           {
                               Curso = d.NombreCurso,
                               CursoID = d.CursoId,
                               e.Asistio,
                               e.EmpleadoAsistenciaId
                           }).ToList();


            var listado = (from a in listado_temp
                           group a by a.CursoID into grp
                           select new
                           {
                               grp.FirstOrDefault().Curso,
                               Asistieron = grp.GroupBy( x => x.EmpleadoAsistenciaId).Where(y => y.FirstOrDefault().Asistio.HasValue ? y.FirstOrDefault().Asistio.Value == AsistieronID : false).Count(),
                               Faltaron = grp.GroupBy(x => x.EmpleadoAsistenciaId).Where(y => y.FirstOrDefault().Asistio.HasValue ? y.FirstOrDefault().Asistio.Value == FaltaronID : false).Count(),
                               PorIniciar = grp.GroupBy(x => x.EmpleadoAsistenciaId).Where(y => y.FirstOrDefault().Asistio.HasValue == false).Count()
                           }).ToList();



            var Chart = new Chart(800, 600, ChartTheme.Green);
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
            string Categoria = string.IsNullOrWhiteSpace(data.Categoria) ? null : data.Categoria;
            string Area = string.IsNullOrWhiteSpace(data.Area) ? null : data.Area;
            int EmpresaId = 0;
            if (!string.IsNullOrWhiteSpace(data.Empresa))
            {
                EmpresaId = (from a in ctx.Empresas where a.RazonSocial == data.Empresa select a.EmpresaId).FirstOrDefault();
            }
            int NoEsEliminado = (int)Enumeradores.EsEliminado.No;

            int AprobaronID = (int)Enumeradores.Condicion.Aprobado;
            int DesaprobaronID = (int)Enumeradores.Condicion.Desaprobado;
            int PorIniciarID = (int)Enumeradores.Condicion.PorIniciar;

            bool validfi = DateTime.TryParseExact(data.FechaInicio, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fi);
            bool validff = DateTime.TryParseExact(data.FechaFin, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ff);

            var listado_temp = (from a in ctx.EmpleadoCursos
                           join b in ctx.SalonProgramados on a.SalonProgramadoId equals b.SalonProgramadoId
                           join c in ctx.CursosProgramados on b.CursoProgramadoId equals c.CursoProgramadoId
                           join d in ctx.Cursos on c.CursoId equals d.CursoId
                           join e in ctx.Empleados on a.EmpleadoId equals e.EmpleadoId
                           join f in ctx.Eventos on c.EventoId equals f.EventoId
                           join g in ctx.Personas on e.PersonaId equals g.PersonaId
                           join h in ctx.EmpleadoAsistencias on a.EmpleadoCursoId equals h.EmpleadoCursoId
                           where
                           (data.CursoId == -1 || d.CursoId == data.CursoId) &&
                           (data.EventoId == -1 || c.EventoId == data.EventoId) &&
                           (data.SedeId == -1 || f.SedeId == data.SedeId) &&
                           (NombreEmpleado == null || (g.Nombres + " " + g.ApellidoPaterno + " " + g.ApellidoMaterno).Contains(NombreEmpleado) || g.NroDocumento.Contains(NombreEmpleado)) &&
                           (data.Condicion == -1 || data.Condicion == a.CondicionId) &&
                           (data.Asistencia == -1 || data.Asistencia == h.Asistio) &&
                           (Area == null ^ e.Area == Area) &&
                           (Categoria == null ^ e.Cargo == Categoria) &&
                           (EmpresaId == 0 || a.EmpresaId == EmpresaId) &&
                           (!validfi || c.FechaInicio >= fi) &&
                            (!validff || c.FechaInicio <= ff) &&
                           (data.CapacitadorId == -1 || b.CapacitadorId == data.CapacitadorId) &&
                           a.EsEliminado == NoEsEliminado
                           select new
                           {
                               Curso = d.NombreCurso,
                               d.CursoId,
                               a.CondicionId
                           }).ToList();

            var listado = (from a in listado_temp
                           group a by a.CursoId into grp
                           select new
                           {
                               grp.FirstOrDefault().Curso,
                               Aprobaron = grp.Where(x => x.CondicionId == AprobaronID).Count(),
                               Desaprobaron = grp.Where(x => x.CondicionId == DesaprobaronID).Count(),
                               PorIniciar = grp.Where(x => x.CondicionId == PorIniciarID).Count()
                           }).ToList();

            var Chart = new Chart(800, 600, ChartTheme.Green);
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
            string Categoria = string.IsNullOrWhiteSpace(data.Categoria) ? null : data.Categoria;
            string Area = string.IsNullOrWhiteSpace(data.Area) ? null : data.Area;
            int EmpresaId = 0;
            if (!string.IsNullOrWhiteSpace(data.Empresa))
            {
                EmpresaId = (from a in ctx.Empresas where a.RazonSocial == data.Empresa select a.EmpresaId).FirstOrDefault();
            }
            int NoEsEliminado = (int)Enumeradores.EsEliminado.No;

            bool validfi = DateTime.TryParseExact(data.FechaInicio, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fi);
            bool validff = DateTime.TryParseExact(data.FechaFin, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ff);

            var listado_temp = (from a in ctx.EmpleadoCursos
                           join b in ctx.SalonProgramados on a.SalonProgramadoId equals b.SalonProgramadoId
                           join c in ctx.CursosProgramados on b.CursoProgramadoId equals c.CursoProgramadoId
                           join d in ctx.Cursos on c.CursoId equals d.CursoId
                           join e in ctx.Empleados on a.EmpleadoId equals e.EmpleadoId
                           join f in ctx.Eventos on c.EventoId equals f.EventoId
                           join g in ctx.Personas on e.PersonaId equals g.PersonaId
                           join h in ctx.EmpleadoAsistencias on a.EmpleadoCursoId equals h.EmpleadoCursoId
                           where
                           (data.CursoId == -1 || d.CursoId == data.CursoId) &&
                           (data.EventoId == -1 || c.EventoId == data.EventoId) &&
                           (data.SedeId == -1 || f.SedeId == data.SedeId) &&
                           (NombreEmpleado == null || (g.Nombres + " " + g.ApellidoPaterno + " " + g.ApellidoMaterno).Contains(NombreEmpleado) || g.NroDocumento.Contains(NombreEmpleado)) &&
                           (data.Asistencia == -1 || data.Asistencia == h.Asistio) &&
                           (data.Condicion == -1 || data.Condicion == a.CondicionId) &&
                           (Area == null || e.Area == Area) &&
                           (Categoria == null || e.Cargo == Categoria) &&
                           (EmpresaId == 0 || a.EmpresaId == EmpresaId) &&
                           (!validfi || c.FechaInicio >= fi) &&
                            (!validff || c.FechaInicio <= ff) &&
                           (data.CapacitadorId == -1 || b.CapacitadorId == data.CapacitadorId) &&
                           a.EsEliminado == NoEsEliminado
                           select new
                           {
                               d.CursoId,
                               Curso = d.NombreCurso,
                               Nota = a.NotaFinal
                           }).ToList();

            var listado = (from a in listado_temp
                           group a by a.CursoId into grp
                           select new
                           {
                               grp.FirstOrDefault().Curso,
                               Promedio = grp.Select(x => x.Nota).Sum() / (decimal)grp.Select(x => x.Nota).Count()
                           }).ToList();


            var Chart = new Chart(800, 600, ChartTheme.Green);
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
                TemplateCell.SetCellValue("Colaborador");
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
                TemplateCell.SetCellValue("Curso");
                indexcell++;

                TemplateCell = TemplateRow.CreateCell(indexcell);
                TemplateCell.CellStyle = CeldaTitulo.CellStyle;
                TemplateCell.SetCellValue("Empresa");
                indexcell++;

                for (int i = 0; i < data.MaximasAsistencias; i++)
                {
                    TemplateCell = TemplateRow.CreateCell(indexcell);
                    TemplateCell.CellStyle = CeldaTitulo.CellStyle;
                    TemplateCell.SetCellValue("A " + (i + 1));
                    indexcell++;
                }

                TemplateCell = TemplateRow.CreateCell(indexcell);
                TemplateCell.CellStyle = CeldaTitulo.CellStyle;
                TemplateCell.SetCellValue("Examen Teorico");
                indexcell++;

                TemplateCell = TemplateRow.CreateCell(indexcell);
                TemplateCell.CellStyle = CeldaTitulo.CellStyle;
                TemplateCell.SetCellValue("Nota Taller");
                indexcell++;

                TemplateCell = TemplateRow.CreateCell(indexcell);
                TemplateCell.CellStyle = CeldaTitulo.CellStyle;
                TemplateCell.SetCellValue("Nota Final");
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
                    TemplateCell.SetCellValue(Alumno.Curso);
                    indexcell++;

                    TemplateCell = TemplateRow.CreateCell(indexcell);
                    TemplateCell.CellStyle = CeldaNormal.CellStyle;
                    TemplateCell.SetCellValue(Alumno.Empresa);
                    indexcell++;

                    for (int i = 0; i < data.MaximasAsistencias; i++)
                    {
                        TemplateCell = TemplateRow.CreateCell(indexcell);
                        TemplateCell.CellStyle = CeldaNormal.CellStyle;
                        TemplateCell.SetCellValue(Alumno.Asistencia.Count <= i ? "" : Alumno.Asistencia[i]);
                        indexcell++;
                    }

                    TemplateCell = TemplateRow.CreateCell(indexcell);
                    TemplateCell.CellStyle = CeldaNormal.CellStyle;
                    TemplateCell.SetCellValue(Alumno.Nota.ToString());
                    indexcell++;

                    TemplateCell = TemplateRow.CreateCell(indexcell);
                    TemplateCell.CellStyle = CeldaNormal.CellStyle;
                    TemplateCell.SetCellValue(Alumno.TallerValor.HasValue ? Alumno.TallerValor.Value.ToString() : "");
                    indexcell++;

                    TemplateCell = TemplateRow.CreateCell(indexcell);
                    TemplateCell.CellStyle = CeldaNormal.CellStyle;
                    TemplateCell.SetCellValue(Alumno.NotaFinal.ToString());
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

        public List<string> GetAreaAutocomplete(string valor)
        {
            List<string> data = (from a in ctx.Empleados select a.Area).ToList().Select(x => x.ToUpper()).Distinct().Where(x => x.Contains(valor.ToUpper())).ToList();

            return data;
        }

        public List<string> GetCategoriaAutocomplete(string valor)
        {
            List<string> data = (from a in ctx.Empleados select a.Cargo).ToList().Select(x => x.ToUpper()).Distinct().Where(x => x.Contains(valor.ToUpper())).ToList();

            return data;
        }

        public List<string> GetEmpresaAutocomplete(string valor)
        {
            List<string> data = (from a in ctx.Empresas select a.RazonSocial).ToList().Select(x => x.ToUpper()).Distinct().Where(x => x.Contains(valor.ToUpper())).ToList();

            return data;
        }
    }
}
