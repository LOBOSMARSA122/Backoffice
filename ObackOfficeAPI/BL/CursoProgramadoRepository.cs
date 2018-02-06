﻿using BE.Administracion;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class CursoProgramadoRepository
    {
        private DatabaseContext ctx = new DatabaseContext();

        public List<Agenda> GetAgenda(int eventoId)
        {
            try
            {
                var query = (from a in ctx.CursosProgramados
                             join b in ctx.Eventos on a.EventoId equals b.EventoId
                             join c in ctx.Parametros on new {a=a.CursoId , b= 103 } equals new {a= c.ParametroId , b=c.GrupoId}
                             where a.EventoId == eventoId
                             select new Agenda
                             {
                                 CursoProgramadoId = a.CursoProgramadoId,
                                 EventoId = a.EventoId,
                                 Evento = b.Nombre,
                                 CursoId = a.CursoId,
                                 Curso = c.Valor1,
                                 FechaInicio = a.FechaInicio,
                                 FechaFin = a.FechaFin
                             }).ToList();
                return query;
            }
            catch (Exception ex)
            {

                throw;
            }
          
        }

    }
}