using APP_TEST_ALTIORACORP.Data;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace APP_TEST_ALTIORACORP.Models.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PedidosApiController : Controller
    {
        private AltioraContext contexto;

        public PedidosApiController(AltioraContext context) {
            this.contexto = context;
        }

        [HttpGet]
        public IActionResult Seleccionar(DataSourceLoadOptions loadOptions) {
            var pedidos = contexto.Pedidos.Select(i => new {
                i.ID,
                i.CLIENTE,
                i.IDPRODUCTO,
                i.CANTIDAD,
                i.FECHAPEDIDO,
                i.PRECIOUNITARIO,
                i.TOTAL
            });
            return Json(DataSourceLoader.Load(pedidos, loadOptions));
        }

        [HttpPost]
        public IActionResult Insertar(string values) {
            var model = new Pedidos();
            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = contexto.Pedidos.Add(model);
            contexto.SaveChanges();

            return Json(result.Entity.ID);
        }

        [HttpPut]
        public IActionResult Actualizar(int key, string values) {
            var model = contexto.Pedidos.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return StatusCode(409, "Pedidos not found");

            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));
            /*
            contexto.Pedidos.Attach(model);
            contexto.Entry(model).State = EntityState.Modified;
            */
            contexto.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public void Eliminar(int key) {
            var model = contexto.Pedidos.FirstOrDefault(item => item.ID == key);

            contexto.Pedidos.Remove(model);
            contexto.SaveChanges();
        }


        [HttpGet]
        public IActionResult ClientesLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in contexto.Clientes
                         orderby i.NOMBRES
                         select new {
                             IDENTIFICACION = i.IDENTIFICACION,
                             NOMBRE = i.NOMBRES,
                             APELLIDOS = i.APELLIDOS
                         };
            return Json(DataSourceLoader.Load(lookup, loadOptions));
        }

        [HttpGet]
        public IActionResult ProductosLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in contexto.Productos
                         orderby i.DESCRIPCION
                         select new {
                             ID = i.ID,
                             DESCRIPCION = i.DESCRIPCION
                         };
            return Json(DataSourceLoader.Load(lookup, loadOptions));
        }

        private void PopulateModel(Pedidos model, IDictionary values) {
            string CLIENTE = nameof(Pedidos.CLIENTE);
            string PRODUCTO = nameof(Pedidos.IDPRODUCTO);
            string CANTIDAD = nameof(Pedidos.CANTIDAD);
            string FECHAPEDIDO = nameof(Pedidos.FECHAPEDIDO);

            if(values.Contains(CLIENTE)) {
                model.CLIENTE = Convert.ToString(values[CLIENTE]);
            }

            if(values.Contains(PRODUCTO)) {
                model.IDPRODUCTO = Convert.ToInt32(values[PRODUCTO]);
            }

            if(values.Contains(CANTIDAD)) {
                model.CANTIDAD = Convert.ToInt32(values[CANTIDAD]);
            }

            if(values.Contains(FECHAPEDIDO)) {
                model.FECHAPEDIDO = Convert.ToDateTime(values[FECHAPEDIDO]);
            }

        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();

            foreach(var entry in modelState) {
                foreach(var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
    }
}