using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace codigoControlMSDOS
{
    class Program
    {
        static string[] diccionario = { "0","1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", 
 "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", 
 "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "+", "/" };
        static void Main(string[] args)
        {
            if (args.Length >= 6) {
                string f = args[0];
                string n = args[1];
                string fe = args[2];
                string m = args[3];
                string a = args[4];
                string l = args[5];
                Console.WriteLine(generarCodigoControl(f,n,fe,m,a,l));
            }
        }
        static public string generarCodigoControl(string factura, string nit, string fecha, string monto, string autorizacion, string llave)
        {
            nit = nit.Trim();
            nit = (nit == "0" || nit == "") ? "0" : nit;
            long fvh = long.Parse(Verhoeff.generateVerhoeff(factura, 2));
            long nvh = long.Parse(Verhoeff.generateVerhoeff(nit, 2));
            long fevh = long.Parse(Verhoeff.generateVerhoeff(fecha, 2));
            long mvh = long.Parse(Verhoeff.generateVerhoeff(((long)Math.Round(double.Parse(monto.Replace('.', ',')))).ToString(), 2));
            long total = fvh + nvh + fevh + mvh;
            string vth = Verhoeff.generateVerhoeff(total.ToString(), 5, true);
            if (vth.Length == 5)
            {
                int nc1 = int.Parse(vth[0].ToString()) + 1;
                int nc2 = int.Parse(vth[1].ToString()) + 1;
                int nc3 = int.Parse(vth[2].ToString()) + 1;
                int nc4 = int.Parse(vth[3].ToString()) + 1;
                int nc5 = int.Parse(vth[4].ToString()) + 1;
                string c1 = autorizacion + llave.Substring(0, nc1);
                string c2 = fvh.ToString() + llave.Substring(nc1, nc2);
                string c3 = (nit == "0" ? "0" : "") + nvh.ToString() + llave.Substring(nc1 + nc2, nc3);
                string c4 = fevh.ToString() + llave.Substring(nc1 + nc2 + nc3, nc4);
                string c5 = mvh.ToString() + llave.Substring(nc1 + nc2 + nc3 + nc4, nc5);
                string ct = c1 + c2 + c3 + c4 + c5;
                string cl = llave + vth;
                string all = allegedrc4(ct, cl);
                int st = sumaDigitosAscii(all);
                int st1 = sumaPosiciones(all, 1);
                int st2 = sumaPosiciones(all, 2);
                int st3 = sumaPosiciones(all, 3);
                int st4 = sumaPosiciones(all, 4);
                int st5 = sumaPosiciones(all, 5);
                long m1 = (st * st1) / nc1;
                long m2 = (st * st2) / nc2;
                long m3 = (st * st3) / nc3;
                long m4 = (st * st4) / nc4;
                long m5 = (st * st5) / nc5;
                long smt = m1 + m2 + m3 + m4 + m5;
                string bsmt = EncodeTo64(smt);
                string codigoControl = allegedrc4(bsmt, cl);
                return codigoControl;
            }
            return "";
        }


        static public string EncodeTo64(long num)
        {
            string t = "";
            long div = num;
            do
            {
                int res = (int)(div % 64);
                t = diccionario[res] + t;
                div = div / 64;
            } while (div > 0);
            return t;
        }

        static public int sumaPosiciones(string cad, int pos)
        {
            int suma = 0;
            pos = pos >= 0 ? pos : 1;
            pos = pos >= cad.Length ? cad.Length : pos;
            pos--;
            for (int i = pos; i < cad.Length; i += 5)
            {
                suma += (int)cad[i];
            }
            return suma;
        }

        static public int sumaDigitosAscii(string cad)
        {
            int suma = 0;
            for (int i = 0; i < cad.Length; i++)
            {
                suma += (int)cad[i];
            }
            return suma;
        }

        static public string allegedrc4(string codigo, string llavellegada)
        {
            int[] State = new int[256 + 1];
            string Mensaje = String.Empty;
            string llave = String.Empty;
            string MsgCif = String.Empty;
            int X = 0;
            int Y = 0;
            int Index1 = 0;
            int Index2 = 0;
            int NMen = 0;
            int i = 0;
            short op1 = 0;
            int aux = 0;
            int op2 = 0;
            string nrohex = String.Empty;


            X = 0;
            Y = 0;
            Index1 = 0;
            Index2 = 0;
            Mensaje = codigo;
            llave = llavellegada;
            for (i = 0; i <= 255.0; i += 1)
            {
                State[i] = i;
            }
            for (i = 0; i <= 255.0; i += 1)
            {
                op1 = (short)(llave.Substring(Index1 + 1 - 1, 1)[0]);
                Index2 = (op1 + State[i] + Index2) % 256;
                aux = State[i];
                State[i] = State[Index2];
                State[Index2] = aux;
                Index1 = (Index1 + 1) % llave.Length;
            }
            for (i = 0; i <= Mensaje.Length - 1; i += 1)
            {
                X = (X + 1) % 256;
                Y = (State[X] + Y) % 256;
                aux = State[X];
                State[X] = State[Y];
                State[Y] = aux;
                op1 = (short)(Mensaje.Substring(i + 1 - 1, 1)[0]);
                op2 = State[(State[X] + State[Y]) % 256];
                NMen = op1 ^ op2;
                nrohex = NMen.ToString("X");
                if (nrohex.Length == 1)
                {
                    nrohex = "0" + nrohex;
                }
                MsgCif = MsgCif + nrohex;
            }
            MsgCif = MsgCif.Substring(MsgCif.Length - (MsgCif.Length));
            return MsgCif;
        }
    }
}
