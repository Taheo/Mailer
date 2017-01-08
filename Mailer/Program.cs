using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Topshelf;
using System.IO;

namespace Mailer
{
    public class Mailer
    {
        public Mailer(){}

        public List<string> emails = new List<string>();
        public List<string> logs = new List<string>();
        public void ReadCSV()
        {
            try
            {   
                using (StreamReader sr = new StreamReader("emails.csv"))
                {
                    while (!sr.EndOfStream)
                    {
                        emails.Add(sr.ReadLine());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(ex.Message);
            }
        }
        //private void ReadTXT()
        //{
        //    try
        //    {   // Open the text file using a stream reader.

        //        using (StreamReader sr = new StreamReader("logi.txt"))
        //        {
        //            // Read the stream to a string, and write the string to the console.
        //            while (!sr.EndOfStream)
        //            {
        //                logs.Add(sr.ReadLine());
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("The file could not be read:");
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        //void SaveLogs()
        //{
        //    try
        //    {
        //        using (StreamWriter outputFile = new StreamWriter("logi.txt"))
        //        {
        //            foreach (string item in logs)
        //                outputFile.WriteLine(item);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}
        public void SendAll(List<string> emails, List<string> logi)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("hhh@gmail.com");
                mail.Subject = "Hi";
                mail.Body = "<h1><i>Hello</i></h1><br /><p>Najlepsze życzenia z okazji świąt</p>";
                mail.IsBodyHtml = true;
                foreach (string item in emails)
                {
                    mail.To.Add(item);
                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new System.Net.NetworkCredential("hhh@gmail.com", "hhh");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                        Console.WriteLine("poszło");
                        logi.Add(item);
                    }
                }
            }
        }
        public void Start()
        {
            Console.WriteLine("work");
        }

        public void Stop()
        {
            Console.WriteLine("stop work");
        }
    }

    public class Program
    {
        public static void Main()
        {
            
            Mailer list = new Mailer();
            HostFactory.Run(x =>                                 
            {
                //x.UseNLog();
                x.Service<Mailer>
                (s =>                        
                    {
                        s.ConstructUsing(name => new Mailer());
                        list.ReadCSV();
                        s.BeforeStartingService(tc => list.Start());
                        s.WhenStarted( tc => list.SendAll(list.emails, list.logs));
                        s.WhenStopped(tc => tc.Stop());               
                    });
                x.RunAsLocalSystem();                            
                x.SetDescription("Mailer with wishes");        
                x.SetDisplayName("Mailer");                       
                x.SetServiceName("Mailer");                       
            });      
                                                        
        }
    }
}