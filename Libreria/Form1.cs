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
        private static int id = 0;
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
        private async void GetLibroById(int id)
        {
            using(var client = new HttpClient())
            {
                string uri = url + "/" + id.ToString();
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var libroJsonString = await response.Content.ReadAsStringAsync();
                    LibrosViewModel oLibro = JsonConvert.DeserializeObject<LibrosViewModel>(libroJsonString);
                    txtAutor.Text = oLibro.Autor;
                    txtEditorial.Text = oLibro.Editorial;
                    txtISBN.Text = oLibro.ISBN;
                    txtTemas.Text = oLibro.Temas;
                    txtTitulo.Text = oLibro.Titulo;
                }
                else
                {
                    MessageBox.Show($"No se puede leer el libro: {response.StatusCode}");
                }
            }
        }
        private void Limpiar()
        {
            txtAutor.Clear();
            txtEditorial.Clear();
            txtISBN.Clear();
            txtTemas.Clear();
            txtTitulo.Clear();
            id = 0;
        }
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            AddLibro();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                if(row.Index == e.RowIndex)
                {
                    id = (int)row.Cells[0].Value;
                    GetLibroById(id);
                }
            }
        }

        
    }
}
