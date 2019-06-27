using APP_TEST_ALTIORACORP.Data;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace APP_TEST_ALTIORACORP.Models.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PedidosApiController : Controller
    {
        private AltioraContext _context;

        public PedidosApiController(AltioraContext context) {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions) {
            var pedidos = _context.Pedidos.Select(i => new {
                i.ID,
                i.CLIENTE,
                i.PED_PRODUCTO,
                i.PED_CANTIDAD,
                i.FECHAPEDIDO,
                i.PED_PRECIO_UNITARIO,
                i.PED_TOTAL
            });
            return Json(DataSourceLoader.Load(pedidos, loadOptions));
        }

        [HttpPost]
        public IActionResult Post(string values) {
            var model = new Pedidos();
            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Pedidos.Add(model);
            _context.SaveChanges();

            return Json(result.Entity.ID);
        }

        [HttpPut]
        public IActionResult Put(int key, string values) {
            var model = _context.Pedidos.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return StatusCode(409, "Pedidos not found");

            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public void Delete(int key) {
            var model = _context.Pedidos.FirstOrDefault(item => item.ID == key);

            _context.Pedidos.Remove(model);
            _context.SaveChanges();
        }


        [HttpGet]
        public IActionResult ClientesLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Clientes
                         orderby i.NOMBRES
                         select new {
                             IDENTIFICACION = i.IDENTIFICACION,
                             NOMBRE = i.NOMBRES
                         };
            return Json(DataSourceLoader.Load(lookup, loadOptions));
        }

        [HttpGet]
        public IActionResult ProductosLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Productos
                         orderby i.DESCRIPCION
                         select new {
                             ID = i.ID,
                             DESCRIPCION = i.DESCRIPCION
                         };
            return Json(DataSourceLoader.Load(lookup, loadOptions));
        }

        private void PopulateModel(Pedidos model, IDictionary values) {
            string CLIENTE = nameof(Pedidos.CLIENTE);
            string PRODUCTO = nameof(Pedidos.PED_PRODUCTO);
            string CANTIDAD = nameof(Pedidos.PED_CANTIDAD);
            string FECHAPEDIDO = nameof(Pedidos.FECHAPEDIDO);

            if(values.Contains(CLIENTE)) {
                model.CLIENTE = Convert.ToString(values[CLIENTE]);
            }

            if(values.Contains(PRODUCTO)) {
                model.PED_PRODUCTO = Convert.ToInt32(values[PRODUCTO]);
            }

            if(values.Contains(CANTIDAD)) {
                model.PED_CANTIDAD = Convert.ToInt32(values[CANTIDAD]);
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