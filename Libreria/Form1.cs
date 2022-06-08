using Api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ViewModels;

namespace Libreria
{
    public partial class Form1 : Form
    {
        string url = "https://localhost:44369/Api/Libros";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetAllLibros();
        }

        private async void GetAllLibros()
        {
            using(var cliente = new HttpClient())
            {
                using(var response = await cliente.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonString = await response.Content.ReadAsStringAsync();
                        dataGridView1.DataSource = JsonConvert.DeserializeObject<List<LibrosViewModel>>(jsonString).ToList();
                    }
                }
            }
        }

        private async void AddLibro()
        {
            LibrosViewModel oLibro = new LibrosViewModel()
            {
                ISBN = txtISBN.Text,
                Autor = txtAutor.Text,
                Editorial = txtEditorial.Text,
                Temas = txtTemas.Text,
                Titulo = txtTitulo.Text
            };

            using(var client = new HttpClient())
            {
                var serializedLibro = JsonConvert.SerializeObject(oLibro);
                var content = new StringContent(serializedLibro, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(url, content);
            }
            Limpiar();
            GetAllLibros();
        }
        private void Limpiar()
        {
            txtAutor.Clear();
            txtEditorial.Clear();
            txtISBN.Clear();
            txtTemas.Clear();
            txtTitulo.Clear();
        }
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            AddLibro();
        }
    }
}
