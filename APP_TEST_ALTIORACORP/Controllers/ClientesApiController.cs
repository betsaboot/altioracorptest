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
    public class ClientesApiController : Controller
    {
        private AltioraContext _context;

        public ClientesApiController(AltioraContext context) {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions) {
            var clientes = _context.Clientes.Select(i => new {
                i.IDENTIFICACION,
                i.NOMBRES,
                i.APELLIDOS,
                i.DIRECCION,
                i.TELEFONO
            });
            return Json(DataSourceLoader.Load(clientes, loadOptions));
        }

        [HttpPost]
        public IActionResult Post(string values) {
            var model = new Clientes();
            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Clientes.Add(model);
            _context.SaveChanges();

            return Json(result.Entity.IDENTIFICACION);
        }

        [HttpPut]
        public IActionResult Put(string key, string values) {
            var model = _context.Clientes.FirstOrDefault(item => item.IDENTIFICACION == key);
            if(model == null)
                return StatusCode(409, "Clientes not found");

            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public void Delete(string key) {
            var model = _context.Clientes.FirstOrDefault(item => item.IDENTIFICACION == key);

            _context.Clientes.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(Clientes model, IDictionary values) {
            string IDENTIFICACION = nameof(Clientes.IDENTIFICACION);
            string NOMBRES = nameof(Clientes.NOMBRES);
            string APELLIDOS = nameof(Clientes.APELLIDOS);
            string DIRECCION = nameof(Clientes.DIRECCION);
            string TELEFONO = nameof(Clientes.TELEFONO);

            if(values.Contains(IDENTIFICACION)) {
                model.IDENTIFICACION = Convert.ToString(values[IDENTIFICACION]);
            }

            if(values.Contains(NOMBRES)) {
                model.NOMBRES = Convert.ToString(values[NOMBRES]);
            }

            if(values.Contains(APELLIDOS)) {
                model.APELLIDOS = Convert.ToString(values[APELLIDOS]);
            }

            if(values.Contains(DIRECCION)) {
                model.DIRECCION = Convert.ToString(values[DIRECCION]);
            }

            if(values.Contains(TELEFONO)) {
                model.TELEFONO = Convert.ToString(values[TELEFONO]);
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