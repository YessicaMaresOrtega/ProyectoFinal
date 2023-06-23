using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using PinacotecaProyectoFinal.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PinacotecaProyectoFinal.ViewModels
{
    public class PinacotecaViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Pinacoteca> ListaPinacotecas { get; set; }
          = new ObservableCollection<Pinacoteca>();

        public string? Errores { get; set; }

        public Pinacoteca Pinacoteca { get; set; }

        public string? Vista { get; set; } = "Ver";

        int indice;

        public ICommand AgregarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand CambiarVistaCommand { get; set; }
        public ICommand CancelarCommand { get; set; }





        public PinacotecaViewModel()
        {
            Cargar();
            AgregarCommand = new RelayCommand(AgregarPinacoteca);
            EditarCommand = new RelayCommand(ModificarPinacoteca);
            EliminarCommand = new RelayCommand(EliminarPinacoteca);
            CancelarCommand = new RelayCommand(Cancelar);
            CambiarVistaCommand = new RelayCommand<string>(CambiarVista);
        }

        private void AgregarPinacoteca()
        {
            Errores = "";
            if (string.IsNullOrWhiteSpace(Pinacoteca.Nombre))
            {
                Errores += "Inserte un nombre correcto de pinoteca\n";
            }
            if (string.IsNullOrWhiteSpace(Pinacoteca.Ciudad))
            {
                Errores += "Agregar una ciudad\n";
            }
            if (string.IsNullOrWhiteSpace(Pinacoteca.Direccion))
            {
                Errores += "Agregar una direccion\n";
            }
            if (Pinacoteca.MetrosCuadrados < 10)
            {
                Errores += "Los metros cuadrados no son los Correctos\n";
            }

            if (Errores == "" && ListaPinacotecas != null)
            {
                ListaPinacotecas.Add(Pinacoteca);
                Guardar();
                CambiarVista("Ver");
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        private void ModificarPinacoteca()
        {
            if (ListaPinacotecas != null)
            {
                ListaPinacotecas[indice] = Pinacoteca;
                Guardar();
                CambiarVista("Ver");
            }
        }

        private void EliminarPinacoteca()
        {
            if (Pinacoteca != null && ListaPinacotecas != null)
            {
                ListaPinacotecas.Remove(Pinacoteca);
                Guardar();

                CambiarVista("Ver");
            }
        }

        private void Cancelar()
        {
            Vista = "Ver";
            Errores = "";
            Pinacoteca = null;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Vista"));
        }

        private void CambiarVista(string obj)
        {
            Vista = obj;
            if (Vista == "Agregar")
            {
                Pinacoteca = new Pinacoteca();
            }

            if (Vista == "Editar")
            {
                if (ListaPinacotecas != null)
                {
                    indice = ListaPinacotecas.IndexOf(Pinacoteca);
                }
                if (Pinacoteca != null)
                {   
                    Pinacoteca clon = new Pinacoteca()
                    {
                        Nombre = Pinacoteca.Nombre,
                        Ciudad = Pinacoteca.Ciudad,
                        Direccion = Pinacoteca.Direccion,
                        MetrosCuadrados = Pinacoteca.MetrosCuadrados
                    };
                    Pinacoteca = clon;
                }
                else
                {
                    Vista = "Ver";
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Vista"));
        }
        private void Guardar()
        {
            var json = JsonConvert.SerializeObject(ListaPinacotecas);
            File.WriteAllText("Pinacotecas.json", json);
        }


        private void Cargar()
        {
            if (File.Exists("Pinacotecas.json"))
            {
                var json = File.ReadAllText("Pinacotecas.json");
                var datos = JsonConvert.DeserializeObject<ObservableCollection<Pinacoteca>>(json);
                if (datos != null)
                {
                    ListaPinacotecas = datos;
                }
                else
                {
                    ListaPinacotecas = new ObservableCollection<Pinacoteca>();
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
