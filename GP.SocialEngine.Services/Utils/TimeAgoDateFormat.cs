using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GP.SocialEngine.Utils
{
    public class TimeAgoDateFormat
    {

        public string Format(DateTime date)
        {
            string timeAgoFormat = "";
            DateTime now = DateTime.Now;

            TimeSpan ts = now.Subtract(date);

            if (ts.Days > 0)
            {
                if (ts.Days > 1)
                {
                    if (ts.Days < 4)
                    {
                        timeAgoFormat = String.Format("{0:El dddd a la(\\s) HH:mm}", date);
                    }
                    else
                    {
                        timeAgoFormat = String.Format("{0:dd/MM/yyyy a la(\\s) HH:mm}", date);
                    }
                }
                else
                {
                    timeAgoFormat = String.Format("Ayer a las {0:HH:mm}", date);
                }
            }
            else
            {
                if (ts.Hours > 0)
                {
                    if (ts.Hours > 1)
                    {
                        timeAgoFormat = ("Hace " + ts.Hours + " horas");
                    }
                    else
                    {
                        timeAgoFormat = ("Hace " + ts.Hours + " hora");
                    }
                }
                else
                {
                    if (ts.Minutes > 0)
                    {
                        if (ts.Minutes > 1)
                        {
                            timeAgoFormat = ("Hace " + ts.Minutes + " minutos");
                        }
                        else
                        {
                            timeAgoFormat = ("Hace " + ts.Minutes + " minuto");
                        }
                    }
                    else
                    {
                        if (ts.Seconds > 0)
                        {
                            if (ts.Seconds > 1)
                            {
                                timeAgoFormat = ("Hace unos segundos");
                            }
                            else
                            {
                                timeAgoFormat = ("Hace " + ts.Seconds + " segundo");
                            }
                        }
                        else
                        {
                            timeAgoFormat = ("Hace 2 segundos");
                        }

                    }

                }
            }
            return timeAgoFormat;
        }
    }
}
