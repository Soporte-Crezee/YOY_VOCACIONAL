using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GP.SocialEngine.Utils
{
   public class TimeBeforeDateFormat
    {

       public string Format(DateTime date)
       {
           string timeAgoFormat = "";
           DateTime now = DateTime.Now;

           TimeSpan ts = date.Subtract(now);

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
                   timeAgoFormat = String.Format(" Mañana a las {0:HH:mm}", date);
               }
           }
           else
           {
               if (ts.Hours > 0)
               {
                   if (ts.Hours > 1)
                   {
                       timeAgoFormat = ("En " + ts.Hours + " horas");
                   }
                   else
                   {
                       timeAgoFormat = ("En " + ts.Hours + " hora");
                   }
               }
               else
               {
                   if (ts.Minutes > 0)
                   {
                       if (ts.Minutes > 1)
                       {
                           timeAgoFormat = ("En " + ts.Minutes + " minutos");
                       }
                       else
                       {
                           timeAgoFormat = ("En " + ts.Minutes + " minuto");
                       }
                   }
                   else
                   {
                       if (ts.Seconds > 0)
                       {
                           if (ts.Seconds > 1)
                           {
                               timeAgoFormat = ("En unos segundos");
                           }
                           else
                           {
                               timeAgoFormat = ("En unos segundos");
                           }
                       }
                       else
                       {
                           timeAgoFormat = ("En unos segundos");
                       }

                   }

               }
           }
           return timeAgoFormat;
       }
    }
}
