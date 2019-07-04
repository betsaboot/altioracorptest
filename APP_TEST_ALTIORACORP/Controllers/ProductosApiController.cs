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
    public class ProductosApiController : Controller
    {
        private AltioraContext contexto;

        public ProductosApiController(AltioraContext context) {
            this.contexto = context;
        }

        [HttpGet]
        public IActionResult Seleccionar(DataSourceLoadOptions loadOptions) {
            var productos = contexto.Productos.Select(i => new {
                i.ID,
                i.DESCRIPCION,
                i.PRECIOUNITARIO,
                i.CODIGO,
                i.STOCK
            });
            return Json(DataSourceLoader.Load(productos, loadOptions));
        }

        [HttpPost]
        public IActionResult Insertar(string values) {
            var model = new Productos();
            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = contexto.Productos.Add(model);
            contexto.SaveChanges();

            return Json(result.Entity.ID);
        }

        [HttpPut]
        public IActionResult Actualizar(int key, string values) {
            var model = contexto.Productos.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return StatusCode(409, "Productos not found");

            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            contexto.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public void Eliminar(int key) {
            var model = contexto.Productos.FirstOrDefault(item => item.ID == key);

            contexto.Productos.Remove(model);
            contexto.SaveChanges();
        }


        private void PopulateModel(Productos model, IDictionary values) {
            string DESCRIPCION = nameof(Productos.DESCRIPCION);
            string PRECIOUNITARIO = nameof(Productos.PRECIOUNITARIO);
            string CODIGO = nameof(Productos.CODIGO);
            string STOCK = nameof(Productos.STOCK);


            if (values.Contains(DESCRIPCION)) {
                model.DESCRIPCION = Convert.ToString(values[DESCRIPCION]);
            }

            if(values.Contains(PRECIOUNITARIO)) {
                model.PRECIOUNITARIO = Convert.ToDouble(values[PRECIOUNITARIO]);
            }

            if(values.Contains(CODIGO)) {
                model.CODIGO = Convert.ToString(values[CODIGO]);
            }
            if (values.Contains(STOCK))
            {
                model.STOCK = Convert.ToInt32(values[STOCK]);
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