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

        public PedidosApiController(AltioraContext context)
        {
            this.contexto = context;
        }

        [HttpGet]
        public IActionResult Seleccionar(DataSourceLoadOptions loadOptions)
        {
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
        public IActionResult Insertar(string values)
        {
            var model = new Pedidos();
            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            // ids trae informacion de la base de datos id y stock de productos
            var ids = contexto.Productos.Select(i => new {
                i.STOCK,
                i.ID
            });

            foreach (var item in ids)
            {
                //model trae info de los formularios

                if (item.ID.Equals(model.IDPRODUCTO))
                {

                    if (model.CANTIDAD >= item.STOCK)
                    {
                        ModelState.AddModelError("Error", "NO HAY STOCK DEL PRODUCTO INGRESADO");
                    }
                    else {

                        Productos producto = new Productos();
                        producto = contexto.Productos.FirstOrDefault(data => data.ID == item.ID);
                        contexto.Productos.Attach(producto);
                        producto.STOCK = producto.STOCK - model.CANTIDAD;
                    }

                }
            }

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = contexto.Pedidos.Add(model);
            contexto.SaveChanges();

            return Json(result.Entity.ID);
        }

        [HttpPut]
        public IActionResult Actualizar(int key, string values)
        {
            Productos producto = new Productos();
           
            var model = contexto.Pedidos.FirstOrDefault(item => item.ID == key);
            producto = contexto.Productos.FirstOrDefault(data => data.ID == model.IDPRODUCTO);

            if (model == null)
                return StatusCode(409, "Pedidos not found");

            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);
            producto = contexto.Productos.FirstOrDefault(data => data.ID == model.IDPRODUCTO);

            var ids = contexto.Pedidos.Select(i => new {
                i.CANTIDAD,
                i.ID
            });
            foreach (var items in ids)
            {

                if (items.ID.Equals(key))
                {

                    if (model.CANTIDAD > producto.STOCK)
                    {
                        ModelState.AddModelError("Error", "NO HAY STOCK DEL PRODUCTO INGRESADO");
                    }
                    else
                    {
                        if (model.CANTIDAD > items.CANTIDAD )
                        {
                            contexto.Productos.Attach(producto);
                            int result = model.CANTIDAD-items.CANTIDAD ;
                            producto.STOCK = producto.STOCK - result;

                        }
                        else
                        {
                            if (model.CANTIDAD < items.CANTIDAD)
                            {
                                contexto.Productos.Attach(producto);
                                int result2 = items.CANTIDAD-model.CANTIDAD;
                                producto.STOCK = producto.STOCK + result2;
                            }
                        }

                    }
                }
            }           

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            contexto.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public void Eliminar(int key)
        {
            var model = contexto.Pedidos.FirstOrDefault(item => item.ID == key);
            Productos producto = new Productos();

            //actualiza stock

            producto = contexto.Productos.FirstOrDefault(data => data.ID == model.IDPRODUCTO);
            contexto.Productos.Attach(producto);
            producto.STOCK = producto.STOCK + model.CANTIDAD;

            contexto.Pedidos.Remove(model);
            contexto.SaveChanges();
        }


        [HttpGet]
        public IActionResult ClientesLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in contexto.Clientes
                         orderby i.NOMBRES
                         select new
                         {
                             IDENTIFICACION = i.IDENTIFICACION,
                             NOMBRE = i.NOMBRES,
                             APELLIDOS = i.APELLIDOS
                         };
            return Json(DataSourceLoader.Load(lookup, loadOptions));
        }

        [HttpGet]
        public IActionResult ProductosLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in contexto.Productos
                         orderby i.DESCRIPCION
                         select new
                         {
                             ID = i.ID,
                             DESCRIPCION = i.DESCRIPCION,
                             STOCK = i.STOCK
                         };
            return Json(DataSourceLoader.Load(lookup, loadOptions));
        }

        private void PopulateModel(Pedidos model, IDictionary values)
        {
            string CLIENTE = nameof(Pedidos.CLIENTE);
            string PRODUCTO = nameof(Pedidos.IDPRODUCTO);
            string CANTIDAD = nameof(Pedidos.CANTIDAD);
            string FECHAPEDIDO = nameof(Pedidos.FECHAPEDIDO);

            if (values.Contains(CLIENTE))
            {
                model.CLIENTE = Convert.ToString(values[CLIENTE]);
            }

            if (values.Contains(PRODUCTO))
            {
                model.IDPRODUCTO = Convert.ToInt32(values[PRODUCTO]);
            }

            if (values.Contains(CANTIDAD))
            {
                model.CANTIDAD = Convert.ToInt32(values[CANTIDAD]);
            }

            if (values.Contains(FECHAPEDIDO))
            {
                model.FECHAPEDIDO = Convert.ToDateTime(values[FECHAPEDIDO]);
            }

        }

        private string GetFullErrorMessage(ModelStateDictionary modelState)
        {
            var messages = new List<string>();

            foreach (var entry in modelState)
            {
                foreach (var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }

    }
}