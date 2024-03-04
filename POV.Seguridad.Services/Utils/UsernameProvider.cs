using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Seguridad.Utils
{
    public class UsernameProvider
    {

        private string formatoFecha = "ddMMyy";
        private string caracteres = "ABCDEFGHJKMNPQRSTUVWXYZ";
        private int lastConsecutivo = 0;

        private byte[] contadorConsecutivos;
        private int longitudPalabra = 15;
        private bool primeraVez = true;
        public int LongitudCaracteres { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }

        public UsernameProvider(string firstName, string lastname)
            : this(firstName, lastname, DateTime.Now)
        {
        }

        public UsernameProvider(string firstName, string lastname, DateTime birthday)
            : this(50, 4, firstName, lastname, birthday)
        {
        }

        public UsernameProvider(int longitudCaracteres, byte numeroConsecutivos, string firstName, string lastname, DateTime birthday)
        {
            this.LongitudCaracteres = longitudCaracteres;
            this.FirstName = firstName;
            this.LastName = lastname;
            this.Birthday = birthday;
            int numeroCaracteres = caracteres.Length;
            this.contadorConsecutivos = new byte[numeroConsecutivos];

            this.contadorConsecutivos[0] = 0;
        }

        public string GenerarUsername()
        {
            StringBuilder sUsername = new StringBuilder();

            FirstName = QuitarAcentos(FirstName);
            LastName = QuitarAcentos(LastName);

            FirstName = QuitarSignosPuntuacion(FirstName);
            LastName = QuitarSignosPuntuacion(LastName);

            FirstName = ObtenerPalabra(FirstName);
            LastName = ObtenerPalabra(LastName);


            if (EsLongitudValida())
            {
                sUsername.Append(FirstName);
                sUsername.Append(".");
                sUsername.Append(LastName);
                sUsername.Append(".");
                sUsername.Append(FormatBirthday());

                sUsername.Append(NextConsecutivo());

            }

            return sUsername.ToString();
        }

        private string NextConsecutivo()
        {
            StringBuilder sConsecutivo = new StringBuilder();
            if (primeraVez)
            {
                for (int i = 0; i < contadorConsecutivos.Length; i++)
                {
                    byte miPosicion = contadorConsecutivos[i];
                    if (i <= lastConsecutivo)
                        sConsecutivo.Append(caracteres[miPosicion]);
                    else
                        break;
                }
                primeraVez = false;
                return sConsecutivo.ToString();
            }
            int numeroCaracteres = caracteres.Length - 1;


            bool incrementLastConsecutivo = false;

            //verificamos si tenemos que incrementar el consecutivo
            for (int i = 0; i < contadorConsecutivos.Length; i++)
            {
                byte contador = contadorConsecutivos[i];

                if (i <= lastConsecutivo)
                {
                    if (contador >= numeroCaracteres)
                        incrementLastConsecutivo = true;
                    else
                    {
                        incrementLastConsecutivo = false;
                        break;
                    }
                }
            }

            if (incrementLastConsecutivo)
            {
                lastConsecutivo++;

                if (lastConsecutivo > contadorConsecutivos.Length - 1)
                    throw new Exception("No se pueden generar mas, especifique mas numeros de consecutivos");
                ResetContador();
            }
            //verificamos si hay que brincar de posicion
            for (int i = lastConsecutivo; i >= 0; i--)
            {
                byte contador = contadorConsecutivos[i];
                if (contador >= numeroCaracteres)
                    contadorConsecutivos[i] = 0;
                else
                {
                    contadorConsecutivos[i]++;
                    break;
                }
            }


            for (int i = 0; i < contadorConsecutivos.Length; i++)
            {
                byte miPosicion = contadorConsecutivos[i];
                if (i <= lastConsecutivo)
                    sConsecutivo.Append(caracteres[miPosicion]);
                else
                    break;
            }

            return sConsecutivo.ToString();
        }

        private bool EsLongitudValida()
        {
            StringBuilder sUsername = new StringBuilder();

            sUsername.Append(FirstName);
            sUsername.Append(".");
            sUsername.Append(LastName);
            sUsername.Append(".");
            sUsername.Append(FormatBirthday());
            return sUsername.ToString().Length <= LongitudCaracteres - contadorConsecutivos.Length;
        }

        private string FormatBirthday()
        {
            return Birthday.ToString(formatoFecha);
        }

        private string ObtenerPalabra(string dato)
        {
            dato = dato.Trim();

            int index = dato.IndexOf(" ", 0);
            if (index > 0)
                dato = dato.Substring(0, index);

            if (dato.Length > longitudPalabra)
                dato = dato.Substring(0, longitudPalabra - 1);
            return dato;
        }

        private void ResetContador()
        {
            for (int i = 0; i < contadorConsecutivos.Length; i++)
            {
                contadorConsecutivos[i] = 0;
            }
        }

        private string QuitarAcentos(string dato)
        {
            dato = dato.ToLowerInvariant();

            dato = dato.Replace("á", "a");
            dato = dato.Replace("é", "e");
            dato = dato.Replace("í", "i");
            dato = dato.Replace("ó", "o");
            dato = dato.Replace("ú", "u");
            dato = dato.Replace("ü", "u");
            dato = dato.Replace("ñ", "n");
            return dato;
        }

        private string QuitarSignosPuntuacion(string dato)
        {


            dato = dato.Replace(".", "");
            dato = dato.Replace(",", "");
            dato = dato.Replace(":", "");
            dato = dato.Replace(";", "");
            dato = dato.Replace("-", " ");

            return dato;
        }
    }
}
