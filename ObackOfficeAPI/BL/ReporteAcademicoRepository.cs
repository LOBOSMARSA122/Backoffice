using BE.Comun;
using BE.RegistroNotas;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Web.Helpers;
using System.Linq;

namespace BL
{
    public class ReporteAcademicoRepository
    {
        private DatabaseContext ctx = new DatabaseContext();

        public BandejaReporteAcademico BandejaReporteAcademico(BandejaReporteAcademico data)
        {
            try
            {
                int NoEsEliminado = (int)Enumeradores.EsEliminado.No;
                int TipoDocumentoGroupId = (int)Enumeradores.GrupoParametros.TipoDocumentos;
                int SedeGroupId = (int)Enumeradores.GrupoParametros.Sedes;
                int ConsidionGroupId = (int)Enumeradores.GrupoParametros.Condición;
                string NombreEmpleado = string.IsNullOrWhiteSpace(data.NombreEmpleado) ? null : data.NombreEmpleado;
                string DNIEmpleado = string.IsNullOrWhiteSpace(data.DNIEmpleado) ? null : data.DNIEmpleado;

                var query = (from a in ctx.CursosProgramados
                             join b in ctx.Eventos on a.EventoId equals b.EventoId
                             join c in ctx.Cursos on a.CursoId equals c.CursoId
                             join e in ctx.SalonProgramados on a.CursoProgramadoId equals e.CursoProgramadoId
                             join g in ctx.EmpleadoCursos on e.SalonProgramadoId equals g.SalonProgramadoId
                             join f in ctx.SalonClases on e.SalonProgramadoId equals f.SalonProgramadoId
                             
                             join i in ctx.Empleados on g.EmpleadoId equals i.EmpleadoId
                             join j in ctx.Personas on i.PersonaId equals j.PersonaId
                             join k in ctx.Parametros on new { a = j.TipoDocumentoId, b = TipoDocumentoGroupId } equals new { a = k.ParametroId, b = k.GrupoId }
                             join l in ctx.Parametros on new { a = b.SedeId, b = SedeGroupId } equals new { a = l.ParametroId, b = l.GrupoId }
                             join m in ctx.Parametros on new { a = g.CondicionId, b = ConsidionGroupId } equals new { a = m.ParametroId, b = m.GrupoId }
                             where 
                             (data.SedeId == -1 || data.SedeId == b.SedeId) &&
                             (data.EventoId == -1 || data.EventoId == b.EventoId) &&
                             (data.CursoId == -1 || data.CursoId == a.CursoId) &&
                             (NombreEmpleado == null || (j.Nombres + " " + j.ApellidoPaterno + " " + j.ApellidoMaterno).Contains(NombreEmpleado)) &&
                             (DNIEmpleado == null || j.NroDocumento.Contains(DNIEmpleado)) &&
                             a.EsEliminado == NoEsEliminado &&
                             f.SalonProgramadoId == e.SalonProgramadoId
                             group new { a,b,c,e,f,g,i,j,k,l,m} by g.EmpleadoCursoId into grp
                             select new ReporteAcademicoList
                             {
                                 CursoProgramadoId = grp.FirstOrDefault().a.CursoProgramadoId,
                                 PersonaId = grp.FirstOrDefault().j.PersonaId,
                                 NroDocumento = grp.FirstOrDefault().j.NroDocumento,
                                 TipoDocumento = grp.FirstOrDefault().k.Valor1,
                                 Curso = grp.FirstOrDefault().c.NombreCurso,
                                 Evento = grp.FirstOrDefault().b.Nombre,
                                 Nombre = grp.FirstOrDefault().j.Nombres + " " + grp.FirstOrDefault().j.ApellidoPaterno + " " + grp.FirstOrDefault().j.ApellidoMaterno,
                                 Sede = grp.FirstOrDefault().l.Valor1,
                                 Condicion = grp.FirstOrDefault().m.Valor1,
                                 Nota = grp.FirstOrDefault().g.Nota,
                                 InicioCurso = grp.FirstOrDefault().a.FechaInicio,
                                 FinCurso = grp.FirstOrDefault().a.FechaFin,
                                 Observaciones = grp.FirstOrDefault().g.Observacion
                             }).ToList();

                BandejaReporteAcademico return_data = new BandejaReporteAcademico()
                {
                    Lista = query
                };
                return return_data;
            }
            catch(Exception e)
            {
                return null;
            }   
        }

        public List<ReporteAcademicoListClase> DetalleEmpleado(int PersonaId, int cursoProgramadoId)
        {
            try
            {
                var return_data = (from a in ctx.SalonProgramados
                                   join b in ctx.EventoSalones on a.EventoSalonId equals b.EventoSalonId
                                   join c in ctx.EmpleadoCursos on a.SalonProgramadoId equals c.SalonProgramadoId
                                   join d in ctx.EmpleadoAsistencias on c.EmpleadoCursoId equals d.EmpleadoCursoId
                                   join e in ctx.Empleados on c.EmpleadoId equals e.EmpleadoId
                                   join f in ctx.Parametros on new {a = d.Asistio, b =  109} equals new { a = f.ParametroId, b = f.GrupoId }
                                   where e.PersonaId == PersonaId &&
                                   a.CursoProgramadoId == cursoProgramadoId
                                   select new ReporteAcademicoListClase
                                   {
                                       Asistencia = f.Valor1,
                                       Salon = b.Nombre,
                                       FechaClase = d.FechaClase
                                   }).ToList();

                return return_data;
            }
            catch(Exception e)
            {
                return new List<ReporteAcademicoListClase>();
            }
        }

        public List<EmpleadoTaller> TallerEmpleado(int PersonaId, int cursoProgramadoId)
        {
            try
            {
                var List = (from a in ctx.EmpleadoTalleres
                            join b in ctx.EmpleadoCursos on a.EmpleadoCursoId equals b.EmpleadoCursoId
                            join c in ctx.Empleados on b.EmpleadoId equals c.EmpleadoId
                            join d in ctx.SalonProgramados on b.SalonProgramadoId equals d.SalonProgramadoId
                            where
                            c.PersonaId == PersonaId &&
                            d.CursoProgramadoId == cursoProgramadoId
                            select a).ToList();


                return List;
            }
            catch (Exception e)
            {
                return new List<EmpleadoTaller>();
            }
        }

        public MemoryStream ReporteAcademicoExcel(BandejaReporteAcademico data, FileStream TemplateFile)
        {
            try
            {
                bool templateDeleted = false;
                int TituloPersonaIndex = 99;
                int DatosPersonaIndex = 100;
                int TituloDetalleIndex = 101;

                IWorkbook TemplateBook = new XSSFWorkbook(TemplateFile);
                ISheet TemplateSheet = TemplateBook.GetSheet(TemplateBook.GetSheetName(0));

                ICellStyle estiloDetalle0 = TemplateSheet.GetRow(102).GetCell(0).CellStyle;
                ICellStyle estiloDetalle1 = TemplateSheet.GetRow(102).GetCell(1).CellStyle;
                ICellStyle estiloDetalle2 = TemplateSheet.GetRow(102).GetCell(2).CellStyle;

                ICellStyle estiloConColor = TemplateSheet.GetRow(102).GetCell(6).CellStyle;
                ICellStyle estiloSinCoolor = TemplateSheet.GetRow(102).GetCell(5).CellStyle;

                ParametroRepository pr = new ParametroRepository();
                List<Dropdownlist> Preguntas = pr.GetParametroByGrupoId(105);
                string Pregunta1 = "N/A";
                string Pregunta2 = "N/A";
                string Pregunta3 = "N/A";
                foreach (var P in Preguntas)
                {
                    switch (P.Id)
                    {
                        case 1:
                            {
                                Pregunta1 = P.Value;
                                break;
                            }
                        case 2:
                            {
                                Pregunta2 = P.Value;
                                break;
                            }
                        case 3:
                            {
                                Pregunta3 = P.Value;
                                break;
                            }
                    }
                }


                List<ReporteAcademicoList> Lista = BandejaReporteAcademico(data).Lista;

                int index = 0;
                
                foreach (var Alumno in Lista)
                {
                    IRow TemplateRow = TemplateSheet.CopyRow(TituloPersonaIndex, index);
                    TituloPersonaIndex = index;
                    index++;

                    TemplateRow = TemplateSheet.CopyRow(DatosPersonaIndex, index);
                    DatosPersonaIndex = index;

                    TemplateRow.GetCell(0).SetCellValue(Alumno.Nombre);
                    TemplateRow.GetCell(1).SetCellValue(Alumno.TipoDocumento);
                    TemplateRow.GetCell(2).SetCellValue(Alumno.NroDocumento);
                    TemplateRow.GetCell(3).SetCellValue(Alumno.Sede);
                    TemplateRow.GetCell(4).SetCellValue(Alumno.Evento);
                    TemplateRow.GetCell(5).SetCellValue(Alumno.Curso);
                    TemplateRow.GetCell(6).SetCellValue(Alumno.Nota.ToString());
                    TemplateRow.GetCell(7).SetCellValue(Alumno.Condicion);
                    TemplateRow.GetCell(8).SetCellValue(Alumno.InicioCurso);
                    TemplateRow.GetCell(9).SetCellValue(Alumno.FinCurso);
                    TemplateRow.GetCell(10).SetCellValue(Alumno.Observaciones);
                    index++;

                    TemplateRow = TemplateSheet.CopyRow(TituloDetalleIndex, index);
                    TituloDetalleIndex = index;
                    index++;

                    int indexDetalle = 0;
                    List<ReporteAcademicoListClase> ListaDetalle = DetalleEmpleado(Alumno.PersonaId, Alumno.CursoProgramadoId);
                    if (ListaDetalle.Count == 0)
                    {
                        TemplateRow = TemplateSheet.CreateRow(index + indexDetalle);

                        ICell cell = TemplateRow.CreateCell(0);
                        cell.SetCellValue("No posee Detalle.");
                        cell.CellStyle = estiloDetalle0;

                        cell = TemplateRow.CreateCell(1);
                        cell.CellStyle = estiloDetalle1;

                        cell = TemplateRow.CreateCell(2);
                        cell.CellStyle = estiloDetalle2;

                        indexDetalle++;
                    }
                    else
                    {
                        foreach (ReporteAcademicoListClase Detalle in ListaDetalle)
                        {
                            TemplateRow = TemplateSheet.CreateRow(index + indexDetalle);

                            ICell cell = TemplateRow.CreateCell(0);
                            cell.SetCellValue(Detalle.FechaClase);
                            cell.CellStyle = estiloDetalle0;

                            cell = TemplateRow.CreateCell(1);
                            cell.SetCellValue(Detalle.Salon);
                            cell.CellStyle = estiloDetalle1;

                            cell = TemplateRow.CreateCell(2);
                            cell.SetCellValue(Detalle.Asistencia);
                            cell.CellStyle = estiloDetalle2;

                            indexDetalle++;
                        }
                    }


                    string R1 = "0";
                    string R2 = "0";
                    string R3 = "0";

                    List<EmpleadoTaller> DetalleTaller = TallerEmpleado(Alumno.PersonaId, Alumno.CursoProgramadoId);

                    if (DetalleTaller.Count > 0)
                    {
                        foreach (EmpleadoTaller Taller in DetalleTaller)
                        {
                            switch (Taller.PreguntaId)
                            {
                                case 1:
                                    {
                                        R1 = Taller.Valor == null ? "0" : Taller.Valor.ToUpper();
                                        break;
                                    }
                                case 2:
                                    {
                                        R2 = Taller.Valor == null ? "0" : Taller.Valor.ToUpper();
                                        break;
                                    }
                                case 3:
                                    {
                                        R3 = Taller.Valor == null ? "0" : Taller.Valor.ToUpper();
                                        break;
                                    }
                            }
                        }
                    }

                    int indexTaller = 0;

                    if(TemplateSheet.GetRow(index + indexTaller) == null)
                    {
                        TemplateRow = TemplateSheet.CreateRow(index + indexTaller);
                    }
                    else
                    {
                        TemplateRow = TemplateSheet.GetRow(index + indexTaller);
                    }

                    ICell cell2 = TemplateRow.CreateCell(5);
                    cell2.SetCellValue(Pregunta1);
                    cell2.CellStyle = estiloSinCoolor;

                    cell2 = TemplateRow.CreateCell(6);
                    cell2.SetCellValue("A");
                    cell2.CellStyle = R1 == "A" ? estiloConColor : estiloSinCoolor;

                    cell2 = TemplateRow.CreateCell(7);
                    cell2.SetCellValue("B");
                    cell2.CellStyle = R1 == "B" ? estiloConColor : estiloSinCoolor;

                    cell2 = TemplateRow.CreateCell(8);
                    cell2.SetCellValue("C");
                    cell2.CellStyle = R1 == "C" ? estiloConColor : estiloSinCoolor;

                    cell2 = TemplateRow.CreateCell(9);
                    cell2.SetCellValue("D");
                    cell2.CellStyle = R1 == "D" ? estiloConColor : estiloSinCoolor;

                    cell2 = TemplateRow.CreateCell(10);
                    cell2.SetCellValue("E");
                    cell2.CellStyle = R1 == "E" ? estiloConColor : estiloSinCoolor;

                    indexTaller++;

                    if (TemplateSheet.GetRow(index + indexTaller) == null)
                    {
                        TemplateRow = TemplateSheet.CreateRow(index + indexTaller);
                    }
                    else
                    {
                        TemplateRow = TemplateSheet.GetRow(index + indexTaller);
                    }

                    cell2 = TemplateRow.CreateCell(5);
                    cell2.SetCellValue(Pregunta2);
                    cell2.CellStyle = estiloSinCoolor;

                    cell2 = TemplateRow.CreateCell(6);
                    cell2.SetCellValue("A");
                    cell2.CellStyle = R2 == "A" ? estiloConColor : estiloSinCoolor;

                    cell2 = TemplateRow.CreateCell(7);
                    cell2.SetCellValue("B");
                    cell2.CellStyle = R2 == "B" ? estiloConColor : estiloSinCoolor;

                    cell2 = TemplateRow.CreateCell(8);
                    cell2.SetCellValue("C");
                    cell2.CellStyle = R2 == "C" ? estiloConColor : estiloSinCoolor;

                    cell2 = TemplateRow.CreateCell(9);
                    cell2.SetCellValue("D");
                    cell2.CellStyle = R2 == "D" ? estiloConColor : estiloSinCoolor;

                    cell2 = TemplateRow.CreateCell(10);
                    cell2.SetCellValue("E");
                    cell2.CellStyle = R2 == "E" ? estiloConColor : estiloSinCoolor;

                    indexTaller++;

                    if (TemplateSheet.GetRow(index + indexTaller) == null)
                    {
                        TemplateRow = TemplateSheet.CreateRow(index + indexTaller);
                    }
                    else
                    {
                        TemplateRow = TemplateSheet.GetRow(index + indexTaller);
                    }

                    cell2 = TemplateRow.CreateCell(5);
                    cell2.SetCellValue(Pregunta3);
                    cell2.CellStyle = estiloSinCoolor;

                    cell2 = TemplateRow.CreateCell(6);
                    cell2.SetCellValue("A");
                    cell2.CellStyle = R3 == "A" ? estiloConColor : estiloSinCoolor;

                    cell2 = TemplateRow.CreateCell(7);
                    cell2.SetCellValue("B");
                    cell2.CellStyle = R3 == "B" ? estiloConColor : estiloSinCoolor;

                    cell2 = TemplateRow.CreateCell(8);
                    cell2.SetCellValue("C");
                    cell2.CellStyle = R3 == "C" ? estiloConColor : estiloSinCoolor;

                    cell2 = TemplateRow.CreateCell(9);
                    cell2.SetCellValue("D");
                    cell2.CellStyle = R3 == "D" ? estiloConColor : estiloSinCoolor;

                    cell2 = TemplateRow.CreateCell(10);
                    cell2.SetCellValue("E");
                    cell2.CellStyle = R3 == "E" ? estiloConColor : estiloSinCoolor;

                    indexTaller++;

                    index = index + (indexDetalle > indexTaller ? indexDetalle : indexTaller);

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

                TemplateSheet.AutoSizeColumn(0);
                TemplateSheet.AutoSizeColumn(1);
                TemplateSheet.AutoSizeColumn(2);
                TemplateSheet.AutoSizeColumn(3);
                TemplateSheet.AutoSizeColumn(4);
                TemplateSheet.AutoSizeColumn(5);
                TemplateSheet.AutoSizeColumn(6);
                TemplateSheet.AutoSizeColumn(7);
                TemplateSheet.AutoSizeColumn(8);
                TemplateSheet.AutoSizeColumn(9);
                TemplateSheet.AutoSizeColumn(10);

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
                                   join f in ctx.SalonClases on e.SalonProgramadoId equals f.SalonProgramadoId
                                   join d in ctx.EmpleadoAsistencias on g.EmpleadoCursoId equals d.EmpleadoCursoId
                                   join i in ctx.Empleados on g.EmpleadoId equals i.EmpleadoId
                                   join j in ctx.Personas on i.PersonaId equals j.PersonaId
                                   join k in ctx.Parametros on new { a = j.TipoDocumentoId, b = TipoDocumentoGroupId } equals new { a = k.ParametroId, b = k.GrupoId }
                                   join l in ctx.Parametros on new { a = b.SedeId, b = SedeGroupId } equals new { a = l.ParametroId, b = l.GrupoId }
                                   join m in ctx.Parametros on new { a = g.CondicionId, b = ConsidionGroupId } equals new { a = m.ParametroId, b = m.GrupoId }
                                   join o in ctx.Parametros on new { a = d.Asistio , b = AsistenciaGroupId } equals new { a = o.ParametroId, b = o.GrupoId }
                                   where
                                   (data.SedeId == -1 || data.SedeId == b.SedeId) &&
                                   (data.EventoId == -1 || data.EventoId == b.EventoId) &&
                                   (data.CursoId == -1 || data.CursoId == a.CursoId) &&
                                   (NombreEmpleado == null || (j.Nombres + " " + j.ApellidoPaterno + " " + j.ApellidoMaterno).Contains(NombreEmpleado)) &&
                                   (DNIEmpleado == null || j.NroDocumento.Contains(DNIEmpleado)) &&
                                   a.EsEliminado == NoEsEliminado &&
                                   f.SalonProgramadoId == e.SalonProgramadoId
                                   group new { b, c,d, g, j, k, l, m, o } by g.EmpleadoCursoId into grp
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

                if(data.Take > 0)
                    return_data = return_data.Skip(skip).Take(data.Take).ToList();

                int[] X = return_data.Select(x => x.Asistencia.Count).ToArray();
                int? maximo = (from a in X orderby a descending select a).FirstOrDefault();

                data.Lista = return_data;
                data.MaximasAsistencias = maximo.HasValue ? maximo.Value : 1;
                data.TotalRegistros = TotalRegistros;

                return data;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public byte[] ChartAsistencia(List<ReporteMultipleList> Lista)
        {
            var ListaCurso = Lista.Select(x => x.EmpleadoCursoId).ToArray();
            var ListaPersona = Lista.Select(x => x.PersonaId).ToArray();

            int AsistieronID = (int)Enumeradores.Asistencia.Asistio;
            int FaltaronID = (int)Enumeradores.Asistencia.Falto;
            int PorIniciarID = (int)Enumeradores.Asistencia.PorIniciar;


            var listado = (from a in ctx.EmpleadoCursos
                           join b in ctx.SalonProgramados on a.SalonProgramadoId equals b.SalonProgramadoId
                           join c in ctx.CursosProgramados on b.CursoProgramadoId equals c.CursoProgramadoId
                           join d in ctx.Cursos on c.CursoId equals d.CursoId
                           join e in ctx.EmpleadoAsistencias on a.EmpleadoCursoId equals e.EmpleadoCursoId
                           join f in ctx.Empleados on a.EmpleadoId equals f.EmpleadoId
                           where ListaCurso.Contains(a.EmpleadoCursoId) &&
                           ListaPersona.Contains(f.PersonaId)
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

        public byte[] ChartAprobados(List<ReporteMultipleList> Lista)
        {
            int AprobaronID = (int)Enumeradores.Condicion.Aprobado;
            int DesaprobaronID = (int)Enumeradores.Condicion.Desaprobado;
            int PorIniciarID = (int)Enumeradores.Condicion.PorIniciar;
            var ListaCurso = Lista.Select(x => x.EmpleadoCursoId).ToArray();
            var ListaPersona = Lista.Select(x => x.PersonaId).ToArray();

            var listado = (from a in ctx.EmpleadoCursos
                           join b in ctx.SalonProgramados on a.SalonProgramadoId equals b.SalonProgramadoId
                           join c in ctx.CursosProgramados on b.CursoProgramadoId equals c.CursoProgramadoId
                           join d in ctx.Cursos on c.CursoId equals d.CursoId
                           join e in ctx.Empleados on a.EmpleadoId equals e.EmpleadoId
                           where ListaCurso.Contains(a.EmpleadoCursoId) &&
                           ListaPersona.Contains(e.PersonaId)
                           group new { a, d } by d.CursoId into grp
                           select new
                           {
                               Curso = grp.FirstOrDefault().d.NombreCurso,
                               Aprobaron = grp.Where( X => X.a.CondicionId == AprobaronID).Count(),
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

        public byte[] ChartPromedio(List<ReporteMultipleList> Lista)
        {
            var ListaCurso = Lista.Select(x => x.EmpleadoCursoId).ToArray();
            var ListaPersona = Lista.Select(x => x.PersonaId).ToArray();

            var listado = (from a in ctx.EmpleadoCursos
                           join b in ctx.SalonProgramados on a.SalonProgramadoId equals b.SalonProgramadoId
                           join c in ctx.CursosProgramados on b.CursoProgramadoId equals c.CursoProgramadoId
                           join d in ctx.Cursos on c.CursoId equals d.CursoId
                           join e in ctx.Empleados on a.EmpleadoId equals e.EmpleadoId
                           where ListaCurso.Contains(a.EmpleadoCursoId) &&
                           ListaPersona.Contains(e.PersonaId)
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

                List<ReporteMultipleList> dataChartList = data.Lista.Select(x => new ReporteMultipleList() { EmpleadoCursoId = x.EmpleadoCursoId, PersonaId = x.PersonaId }).ToList();
                string[] chartList = data.Charts;

                List<int> ListaCurso = data.Lista.Select(x => x.EmpleadoCursoId).ToList();
                List<int> ListaPersona = data.Lista.Select(x => x.PersonaId).ToList();

                data = BandejaReporteMultiple(data);



                data.Lista = data.Lista.Where(x => ListaCurso.Contains(x.EmpleadoCursoId) && ListaPersona.Contains(x.PersonaId)).ToList();

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

                for(int i = 0; i < data.MaximasAsistencias; i++)
                {
                    TemplateCell = TemplateRow.CreateCell(indexcell);
                    TemplateCell.CellStyle = CeldaTitulo.CellStyle;
                    TemplateCell.SetCellValue("A " + indexcell);
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

                foreach(var chart in chartList)
                {
                    byte[] Image = null;
                    switch (chart)
                    {
                        case "Asistencia":
                            {
                                Image = ChartAsistencia(dataChartList);
                                break;
                            }
                        case "Aprobados":
                            {
                                Image = ChartAprobados(dataChartList);
                                break;
                            }
                        case "Promedio":
                            {
                                Image = ChartAsistencia(dataChartList);
                                break;
                            }
                    }


                    if(Image != null)
                    {
                        int pictureIndex = TemplateBook.AddPicture(Image, PictureType.PNG);
                        ICreationHelper helper = TemplateBook.GetCreationHelper();
                        IDrawing drawing = TemplateSheet.CreateDrawingPatriarch();
                        IClientAnchor anchor = helper.CreateClientAnchor();
                        anchor.Col1 = 0;//0 index based column
                        anchor.Row1 = index;//0 index based row
                        IPicture picture = drawing.CreatePicture(anchor, pictureIndex);
                        picture.Resize();
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
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
