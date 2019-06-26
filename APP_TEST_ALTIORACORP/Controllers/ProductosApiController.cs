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
        private AltioraContext _context;

        public ProductosApiController(AltioraContext context) {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions) {
            var productos = _context.Productos.Select(i => new {
                i.ID,
                i.DESCRIPCION,
                i.PRECIOUNITARIO,
                i.CODIGO
            });
            return Json(DataSourceLoader.Load(productos, loadOptions));
        }

        [HttpPost]
        public IActionResult Post(string values) {
            var model = new Productos();
            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Productos.Add(model);
            _context.SaveChanges();

            return Json(result.Entity.ID);
        }

        [HttpPut]
        public IActionResult Put(int key, string values) {
            var model = _context.Productos.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return StatusCode(409, "Productos not found");

            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public void Delete(int key) {
            var model = _context.Productos.FirstOrDefault(item => item.ID == key);

            _context.Productos.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(Productos model, IDictionary values) {
            string DESCRIPCION = nameof(Productos.DESCRIPCION);
            string PRECIOUNITARIO = nameof(Productos.PRECIOUNITARIO);
            string CODIGO = nameof(Productos.CODIGO);

            if(values.Contains(DESCRIPCION)) {
                model.DESCRIPCION = Convert.ToString(values[DESCRIPCION]);
            }

            if(values.Contains(PRECIOUNITARIO)) {
                model.PRECIOUNITARIO = Convert.ToDouble(values[PRECIOUNITARIO]);
            }

            if(values.Contains(CODIGO)) {
                model.CODIGO = Convert.ToString(values[CODIGO]);
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