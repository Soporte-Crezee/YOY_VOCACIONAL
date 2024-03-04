using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.Expediente.Reports.Reports
{
    class auxiliar
    {
        private string clasificador;
        private int valor;
  

      public void setValor(int valor){
          this.valor=valor;
        }
      public void setClasificador(string clasificador)
      {
          this.clasificador = clasificador;
      }
      public int getValor() {
          return valor;
      }
      public string getClasificador() {
          return clasificador;
      }


     public  auxiliar(string clasificador, int valor) {
            setClasificador(clasificador);
            setValor(valor);
        }


      
    }
}
