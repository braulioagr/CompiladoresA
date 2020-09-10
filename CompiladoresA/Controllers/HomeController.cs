using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CompiladoresA.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult getGramatica(string gramatica)
        {
            bool band = true;
            Dictionary<string, List<string>> produccion;

            gramatica = gramatica.Replace(" ", "");
            produccion = this.calculaGramatica(gramatica.Split('\n').ToArray(), ref band);

            if (band)
            {
                //Regresa una vista con la impresión del diccionario en interfaz
                return PartialView("ImpresionDiccionario", produccion);
            }
            else
            {
                return PartialView("FallaDiccionario");
            }
        }

        [HttpPost]
        public ActionResult getPrimeros(string gramatica)
        {
            bool band = true;
            Dictionary<string, List<string>> produccion;

            gramatica = gramatica.Replace(" ", "");
            produccion = this.calculaGramatica(gramatica.Split('\n').ToArray(), ref band);
            if (band)
            {
                primeros = this.calculaPrimeros(produccion);
                return PartialView("Primeros", primeros);
            }
            else
            {
                return PartialView("FallaDiccionario");
            }
        }

        [HttpPost]
        public ActionResult getSiguientes(string gramatica)
        {
            bool band = true;
            gramatica = gramatica.Replace(" ", "");
            producciones = this.calculaGramatica(gramatica.Split('\n').ToArray(), ref band);
            if (band)
            {
                primeros = this.calculaPrimeros(producciones);
                siguientes = this.calculaSiguientes(producciones);
                return PartialView("ImpresionDiccionario", siguientes);
            }
            else
            {
                return PartialView("FallaDiccionario");
            }
        }

        [HttpPost]
        public ActionResult getADF(string gramatica)
        {
            bool band = true;
            gramatica = gramatica.Replace(" ", "");

            this.producciones = this.calculaGramatica(gramatica.Split('\n').ToArray(), ref band);
            if (band)
            {

                this.primeros = this.calculaPrimeros(producciones);
                this.siguientes = this.calculaSiguientes(producciones);

                this.producciones = this.aumetaGramatica(this.producciones); // Se aumenta la gramatica
                this.terminales = this.obtenTerminales(this.producciones);
                this.noTerminales = this.producciones.Keys.OrderBy(llave => llave).ToList();
                this.totalSimbolos = this.terminales.Count + this.noTerminales.Count - 1;
                List<string> simbolos;
                simbolos = new List<string>();
                foreach (string simbolo in this.terminales)
                {
                    simbolos.Add(simbolo);
                }
                foreach (string simbolo in this.noTerminales)
                {
                    simbolos.Add(simbolo);
                }
                simbolos.Remove(this.producciones.Keys.First());
                this.simbolosGramaticales = simbolos.ToArray();
                this.tablaAFD = new List<string[]>();
                this.conjuntosCanonicos = this.caculaAFD();
                ViewBag.SimbolosGramaticales = this.simbolosGramaticales;
                ViewBag.TablaADF = this.tablaAFD;
                return PartialView("ImpresionLR0", conjuntosCanonicos);
            }
            else
            {
                return PartialView("FallaDiccionario");
            }

        }


        [HttpPost]
        public ActionResult getTabla(string gramatica)
        {
            bool band = true;
            gramatica = gramatica.Replace(" ", "");

            this.producciones = this.calculaGramatica(gramatica.Split('\n').ToArray(), ref band);
            if (band)
            {
                this.produccionesOriginales = this.calculaGramatica(gramatica.Split('\n').ToArray(), ref band);

                this.primeros = this.calculaPrimeros(producciones);
                this.siguientes = this.calculaSiguientes(producciones);

                this.producciones = this.aumetaGramatica(this.producciones); // Se aumenta la gramatica
                this.terminales = this.obtenTerminales(this.producciones);
                this.noTerminales = this.producciones.Keys.OrderBy(llave => llave).ToList();
                this.totalSimbolos = this.terminales.Count + this.noTerminales.Count - 1;
                List<string> simbolos;
                simbolos = new List<string>();
                foreach (string simbolo in this.terminales)
                {
                    simbolos.Add(simbolo);
                }
                foreach (string simbolo in this.noTerminales)
                {
                    simbolos.Add(simbolo);
                }
                simbolos.Remove(this.producciones.Keys.First());
                this.simbolosGramaticales = simbolos.ToArray();
                this.tablaAFD = new List<string[]>();
                this.conjuntosCanonicos = this.caculaAFD();
                simbolos = new List<string>();
                foreach (string simbolo in this.terminales)
                {
                    simbolos.Add(simbolo);
                }
                simbolos.Add("$");
                foreach (string simbolo in this.noTerminales)
                {
                    simbolos.Add(simbolo);
                }
                simbolos.Remove(this.producciones.Keys.First());
                this.simbolosGramaticales = simbolos.ToArray();
                this.tablaDeAnalisis = this.calculaTablaDeAnalisis();
                ViewBag.SimbolosGramaticales = this.simbolosGramaticales;
                ViewBag.TotalEstados = this.conjuntosCanonicos.Count();
                //Regresa una vista con la impresión del diccionario en interfaz
                return PartialView("ImpresionTablaAnalisis", this.tablaDeAnalisis);
            }
            else
            {
                return PartialView("FallaDiccionario");
            }
        }

        [HttpPost]
        public ActionResult getCadena(string cadena, string gramatica)
        {
            bool band = true;
            gramatica = gramatica.Replace(" ", "");
            cadena = cadena.Replace(" ", "");

            this.producciones = this.calculaGramatica(gramatica.Split('\n').ToArray(), ref band);
            if (band)
            {
                this.produccionesOriginales = this.calculaGramatica(gramatica.Split('\n').ToArray(), ref band);

                this.primeros = this.calculaPrimeros(producciones);
                this.siguientes = this.calculaSiguientes(producciones);

                this.producciones = this.aumetaGramatica(this.producciones); // Se aumenta la gramatica
                this.terminales = this.obtenTerminales(this.producciones);
                this.noTerminales = this.producciones.Keys.OrderBy(llave => llave).ToList();
                this.totalSimbolos = this.terminales.Count + this.noTerminales.Count - 1;
                List<string> simbolos;
                simbolos = new List<string>();
                foreach (string simbolo in this.terminales)
                {
                    simbolos.Add(simbolo);
                }
                foreach (string simbolo in this.noTerminales)
                {
                    simbolos.Add(simbolo);
                }
                simbolos.Remove(this.producciones.Keys.First());
                this.simbolosGramaticales = simbolos.ToArray();
                this.tablaAFD = new List<string[]>();
                this.conjuntosCanonicos = this.caculaAFD();
                simbolos = new List<string>();
                foreach (string simbolo in this.terminales)
                {
                    simbolos.Add(simbolo);
                }
                simbolos.Add("$");
                foreach (string simbolo in this.noTerminales)
                {
                    simbolos.Add(simbolo);
                }
                simbolos.Remove(this.producciones.Keys.First());
                this.simbolosGramaticales = simbolos.ToArray();
                this.tablaDeAnalisis = this.calculaTablaDeAnalisis();
                ViewBag.SimbolosGramaticales = this.simbolosGramaticales;
                ViewBag.TotalEstados = this.conjuntosCanonicos.Count();
                ViewBag.TablaDeAnalisis = this.tablaDeAnalisis;

                // AQUI VA LA MAGIA...
                bool valida;
                valida = this.validaCadena(cadena);
                if (valida)
                {
                    valida = evaluarCadena(cadena, new List<int>());
                }
                //Regresa una vista con la impresión del diccionario en interfaz
                return PartialView("ImpresionCadena", valida);
            }
            else
            {
                return PartialView("FallaDiccionario");
            }
        }

        Dictionary<string, List<string>> producciones, produccionesOriginales;
        Dictionary<string, List<string>> primeros;
        Dictionary<string, List<string>> siguientes;
        Dictionary<int, Dictionary<string, List<string>>> conjuntosCanonicos;
        List<string> noTerminales, terminales;
        List<string[]> tablaAFD;
        int totalSimbolos;
        string[] simbolosGramaticales;
        string[,] tablaDeAnalisis;
        string simboloInicial;

        #region Gramatica
        public Dictionary<string, List<string>> calculaGramatica(string[] producciones, ref bool band)
        {
            Dictionary<string, List<string>> datos = new Dictionary<string, List<string>>();
            string[] expresion;
            string key;
            List<string> produccion;
            for (int i = 0; i < producciones.Length; i++) {
                expresion = producciones[i].Split(new string[] { "->" }, StringSplitOptions.None).ToArray();
                key = expresion.First();
                if (char.IsUpper(key.ToString()[0]) && key.Length == 1)
                {
                    if (!datos.ContainsKey(key)) {
                        if (expresion.Last().Contains("|")) {
                            produccion = expresion.Last().Split('|').ToList();
                        } else {
                            produccion = new List<string>();
                            produccion.Add(expresion.Last());
                        }
                        datos.Add(key, produccion);
                    } else {
                        if (expresion.Last().Contains("|")) {
                            produccion = expresion.Last().Split('|').ToList();
                            foreach (string dato in produccion) {
                                datos[key].Add(dato);
                            }
                        } else {
                            datos[key].Add(producciones[i].Substring(producciones[i].IndexOf("->")+2));
                        }
                    }
                }
                else
                {
                    band = false;
                    break;
                }
            }
            return datos;
        }
        #endregion

        #region Primeros
        /**
         * Este metodo es el encargado de calcular el conjunto de primeros progresivamente
         *  calculandolos NoTerminal por NoTerminal.
         * @param Dictionary<string, List<string>> Diccionario de la gramatica asociada
         * @return Diccionario que representa al conjunto de primeros
         */
        public Dictionary<string, List<string>> calculaPrimeros(Dictionary<string, List<string>> gramatica)
        {
            primeros = new Dictionary<string, List<string>>();

            foreach (var produccion in gramatica)
            {
                foreach (var alternativa in produccion.Value)
                {
                    for (int i = 0; i < alternativa.Length; i++)
                    {
                        if (!primeros.ContainsKey(alternativa[i].ToString()) &&
                            !char.IsUpper(alternativa[i]) && alternativa[i].ToString() != "ε")
                        {
                            primeros.Add(alternativa[i].ToString(), new List<string>());
                            primeros[alternativa[i].ToString()].Add(alternativa[i].ToString());
                        }
                    }
                }
            }

            foreach (var produccion in gramatica) // Cada llave debe ser única debido a las producciones
            {
                primeros.Add(produccion.Key, new List<string>());
            }
            var newDictionary = primeros.ToDictionary(entry => entry.Key, entry => (new List<string>(entry.Value)));

            bool equal = false;
            do
            {
                foreach (var llave in gramatica.Keys)
                    primeros[llave] = primerosNoTerminal(llave, gramatica[llave]);

                equal = checaSiSonIguales(newDictionary, primeros);

                newDictionary = primeros.ToDictionary(entry => entry.Key, entry => (new List<string>(entry.Value)));
            } while (equal == false);

            return primeros;
        }

        private bool checaSiSonIguales(Dictionary<string, List<string>> dic1, Dictionary<string, List<string>> dic2) {
            if (dic1.Count == dic2.Count) {
                foreach (var pair in dic1) {
                    List<string> value;
                    if (dic2.TryGetValue(pair.Key, out value)) {
                        if (value.Except(pair.Value).Count() > 0)
                            return false;
                    }
                    else return false;
                }

                return true;
            }

            return false;
        }

        /**
         * Este metodo se encaga de calcular el conjunto de primeros asociados al encabezado que recibe como
         *  parametro.
         * @param string Encabezado que se planea Calcular sus primeros
         * @param List<string> Producciones asociadas al encabezado
         * @return Retorna una lista que representa el subconjunto de primeros asociados al encabezado
         */
        public List<string> primerosNoTerminal(string llave, List<string> producciones)
        {
            bool band;
            List<string> primeros;
            List<string> primerosAux;
            primeros = new List<string>();
            primerosAux = new List<string>();
            producciones = reordenaRecursivas(llave, producciones);
            band = true;
            foreach (string produccion in producciones)
            {
                primerosAux.Clear();
                primerosCadena(produccion, ref primerosAux);
                if (!primerosAux.Contains("ε"))
                    band = false;
                primeros = primeros.Union(primerosAux).ToList().Distinct().ToList();
                // primeros.Remove("ε");
            }
            if ((band || producciones.Contains("ε")) && !primeros.Contains("ε"))
            {
                primeros.Add("ε");
            }
            return primeros;
        }

        /**
         * Este metodo es el encargado de mandar las producciones recursivas al final 
         *  para ser las ultimas en ser evaludadas
         * @param string Encabezado de la produccion
         * @param List<string> Conjunto de producciones asociadas al encabezado
         * @return Retorna las producciones ya reorganizaddas
         */
        private List<string> reordenaRecursivas(string encabezado, List<string> producciones)
        {
            List<string> recursivas;
            List<string> norecursivas;
            recursivas = new List<string>();
            norecursivas = new List<string>();
            foreach (string produccion in producciones)
            {
                if (produccion.First().ToString().Equals(encabezado))
                {
                    recursivas.Add(produccion);
                }
                else
                {
                    norecursivas.Add(produccion);
                }
            }
            producciones.Clear();
            foreach (string produccion in norecursivas)
            {
                producciones.Add(produccion);
            }
            foreach (string produccion in recursivas)
            {
                producciones.Add(produccion);
            }
            return producciones;
        }


        public void primerosCadena(string produccion, ref List<string> primerosAux)
        {
            foreach (var simbolo in produccion)
                if (char.IsUpper(simbolo))
                {
                    primerosAux = primerosAux.Union(primeros[simbolo.ToString()]).ToList().Distinct().ToList();

                    if (!primerosAux.Contains("ε"))
                        return;

                    primerosAux.Remove("ε");
                }
                else
                {
                    if (!primerosAux.Contains(simbolo.ToString()))
                        primerosAux.Add(simbolo.ToString());
                    return;
                }

            primerosAux.Add("ε");
        }

        #endregion

        #region Siguientes
        public Dictionary<string, List<string>> calculaSiguientes(Dictionary<string, List<string>> gramatica)
        {
            siguientes = new Dictionary<string, List<string>>();
            foreach (var produccion in gramatica) // Cada llave debe ser única debido a las producciones
            {
                siguientes.Add(produccion.Key, new List<string>());
            }
            siguientes.First().Value.Add("$");
            var newDictionary = siguientes.ToDictionary(entry => entry.Key, entry => (new List<string>(entry.Value)));

            bool equal = false;
            do
            {
                foreach (var cabeza in gramatica.Keys)
                {
                    siguientesNoTerminal(cabeza, gramatica[cabeza]);
                }

                equal = checaSiSonIguales(newDictionary, siguientes);

                newDictionary = siguientes.ToDictionary(entry => entry.Key, entry => (new List<string>(entry.Value)));//Se actualiza newDicitonary
            } while (equal == false);

            return siguientes;
        }

        private void siguientesNoTerminal(string cabeza, List<string> list)
        {
            List<string> primerosAux = new List<string>();

            foreach (var produccion in list)
            {
                for (int i = 0; i < produccion.Length; i++)
                { // Se evalúan las producciones que tengan al menos un elemento para calc. Prim
                    if (!Char.IsUpper(produccion[i])) // Si es terminal pasamos...
                        continue;

                    primerosAux.Clear();
                    // Se sacan los primeros de los siguientes símbolos
                    primerosCadena(produccion.Substring(i + 1), ref primerosAux);

                    siguientes[produccion[i].ToString()] = siguientes[produccion[i].ToString()].Union(primerosAux).ToList().Distinct().ToList();
                    if (siguientes[produccion[i].ToString()].Contains("ε"))
                        siguientes[produccion[i].ToString()].Remove("ε");

                    if (primerosAux.Contains("ε"))
                        siguientes[produccion[i].ToString()] = siguientes[produccion[i].ToString()].Union(siguientes[cabeza]).ToList().Distinct().ToList();
                }
            }
        }

        #endregion

        #region Automata Finito Determinista
        public List<string> obtenTerminales(Dictionary<string, List<string>> gramatica)
        {
            noTerminales = new List<string>();
            foreach (var produccion in gramatica)
            {
                foreach (string alternativa in produccion.Value)
                {
                    for (int i = 0; i < alternativa.Length; i++)
                    {
                        if (!noTerminales.Contains(alternativa[i].ToString()) && !char.IsUpper(alternativa[i]) && alternativa[i].ToString() != "ε")
                        {
                            noTerminales.Add(alternativa[i].ToString());
                        }
                    }
                }
            }
            noTerminales = noTerminales.OrderBy(cadena => cadena).ToList();
            return noTerminales;
        }

        private int checaSiContieneConjunto(Dictionary<int, Dictionary<string, List<string>>> afd, Dictionary<string, List<string>> conjuntoI) {
            foreach (var conjunto in afd)
                if (checaSiSonIguales(conjunto.Value, conjuntoI))
                    return conjunto.Key;

            return -1;
        }

        private Dictionary<int, Dictionary<string, List<string>>> caculaAFD() // Ya recibe la aumentada
        {
            Dictionary<int, Dictionary<string, List<string>>> afd;
            int j;
            afd = new Dictionary<int, Dictionary<string, List<string>>>();
            Dictionary<string, List<string>> produccionesActuales;
            produccionesActuales = new Dictionary<string, List<string>>();
            produccionesActuales.Add(this.producciones.First().Key, this.agregaPuntoInicio(this.producciones.First().Value)); // Primer NT de la agregada
            afd.Add(0, cerradura(produccionesActuales)); // Se agrega I(0)
            Queue porProcesar, procesados;
            porProcesar = new Queue();
            procesados = new Queue();
            porProcesar.Enqueue(afd[0]);
            Dictionary<string, List<string>> posibleConjuntoI;
            string[] renglon;
            while (porProcesar.Count > 0)
            {
                produccionesActuales = (Dictionary<string, List<string>>)porProcesar.Dequeue();
                procesados.Enqueue(produccionesActuales); // Estado I(J)
                produccionesActuales = desplazaPunto(produccionesActuales);

                renglon = new string[this.totalSimbolos];
                for (int i = 0; i < renglon.Length; i++)
                {
                    renglon[i] = "Φ";
                }

                foreach (string terminal in this.terminales)
                {
                    posibleConjuntoI = cerradura(agrupaPorSimbolo(terminal, produccionesActuales));
                    if (posibleConjuntoI.Count > 0)
                    {
                        int indexConjunto = checaSiContieneConjunto(afd, posibleConjuntoI);
                        if (indexConjunto == -1)
                        {
                            afd.Add(afd.Count, posibleConjuntoI);
                            j = this.findIndex(terminal);
                            renglon[j] = ((afd.Count) - 1).ToString();
                            if (!porProcesar.Contains(posibleConjuntoI) && !procesados.Contains(posibleConjuntoI))
                            {
                                porProcesar.Enqueue(posibleConjuntoI);
                            }
                        }
                        else
                        {
                            j = this.findIndex(terminal);
                            renglon[j] = indexConjunto.ToString();
                        }
                    }
                }
                foreach (string noTerminal in this.noTerminales) {
                    posibleConjuntoI = cerradura(agrupaPorSimbolo(noTerminal, produccionesActuales));
                    if (posibleConjuntoI.Count > 0)
                    {
                        int indexConjunto = checaSiContieneConjunto(afd, posibleConjuntoI);
                        if (indexConjunto == -1)
                        {
                            afd.Add(afd.Count, posibleConjuntoI);
                            j = this.findIndex(noTerminal);
                            renglon[j] = ((afd.Count)-1).ToString();
                            if (!porProcesar.Contains(posibleConjuntoI) && !procesados.Contains(posibleConjuntoI))
                            {
                                porProcesar.Enqueue(posibleConjuntoI);
                            }
                        }
                        else
                        {
                            j = this.findIndex(noTerminal);
                            renglon[j] = indexConjunto.ToString();
                        }
                    }
                }
                this.tablaAFD.Add(renglon);
            }
            return afd;
        }

        private Dictionary<string, List<string>> aumetaGramatica(Dictionary<string, List<string>> producciones)
        {
            Dictionary<string, List<string>> gramatica;
            this.simboloInicial = this.producciones.First().Key;
            gramatica = new Dictionary<string, List<string>>();
            gramatica.Add(producciones.First().Key+ "'", new List<string>());
            gramatica[producciones.First().Key + "'"].Add(this.producciones.First().Key);
            foreach (var produccion in this.producciones)
            {
                gramatica.Add(produccion.Key, produccion.Value);
            }
            return gramatica;
        }
        
        private Dictionary<string, List<string>> cerradura(Dictionary<string, List<string>> producciones2)
        {
            Dictionary<string, List<string>> estado;
            estado = new Dictionary<string, List<string>>();

            Queue porProcesar, procesados;
            porProcesar = new Queue();
            procesados = new Queue();

            int i;
            string A;
            List<string> Beta;
            foreach (var NT in producciones2) {
                if (!estado.ContainsKey(NT.Key))
                    estado.Add(NT.Key, new List<string>());

                foreach (string produccion in NT.Value)
                {
                    if (!estado[NT.Key].Contains(produccion))
                        estado[NT.Key].Add(produccion);

                    i = produccion.IndexOf('.');
                    if (i != produccion.Length - 1) { // Si está al final
                        i++;
                        if (char.IsUpper(produccion[i])) { // Si le sigue un NT
                            A = produccion[i].ToString();

                            if(!porProcesar.Contains(A))
                                porProcesar.Enqueue(A);
                        }
                    }
                }
            }

            while(porProcesar.Count > 0) {
                A = porProcesar.Dequeue().ToString();
                procesados.Enqueue(A);

                if(!estado.ContainsKey(A))
                    estado.Add(A, new List<string>());

                Beta = this.agregaPuntoInicio(producciones[A]); // nuevos!!
                foreach (string cadena in Beta) {
                    if(!estado[A].Contains(cadena))
                        estado[A].Add(cadena);

                    int j = cadena.IndexOf('.')+1;

                    if (j < cadena.Length && char.IsUpper(cadena[j]))
                        if (!porProcesar.Contains(cadena[j].ToString()) && !procesados.Contains(cadena[j].ToString()))
                            porProcesar.Enqueue(cadena[j].ToString());
                }
            }

            return estado;
        }

        private List<string> agregaPuntoInicio(List<string> cadenas)
        {
            List<string> newCadenas;
            newCadenas = new List<string>();

            foreach (string cad in cadenas)
                if(cad == "ε")
                    newCadenas.Add(".");
                else newCadenas.Add("." + cad);

            return newCadenas;
        }

        private Dictionary<string, List<string>> desplazaPunto(Dictionary<string, List<string>> gramatica) {
            Dictionary<string, List<string>> nuevaGrama = new Dictionary<string, List<string>>();

            int i;
            char[] temp;
            foreach (var simbolo in gramatica) {
                foreach (string cadena in simbolo.Value) {
                    i = cadena.IndexOf('.') + 1;

                    if (i < cadena.Length) {
                        temp = cadena.ToCharArray();

                        temp[i - 1] = temp[i];
                        temp[i] = '.';
                        string keyActual = simbolo.Key;

                        if (!nuevaGrama.ContainsKey(keyActual))
                            nuevaGrama.Add(keyActual, new List<string>());
                        if (!nuevaGrama[keyActual].Contains(new string(temp)))
                            nuevaGrama[keyActual].Add(new string(temp));
                    }
                }
            }

            return nuevaGrama;
        }

        // Al recibir la gramatica desplazada no existen epsilons...
        private Dictionary<string, List<string>> agrupaPorSimbolo(string terminal, Dictionary<string, List<string>> gramatica) {
            Dictionary<string, List<string>> nuevaGrama = new Dictionary<string, List<string>>();

            int i;
            foreach (var simbolo in gramatica) {
                foreach (string cadena in simbolo.Value) {
                    i = cadena.IndexOf('.');

                    if(cadena[i-1].ToString() == terminal) {
                        string keyActual = simbolo.Key;

                        if (!nuevaGrama.ContainsKey(keyActual))
                            nuevaGrama.Add(keyActual, new List<string>());
                        if (!nuevaGrama[keyActual].Contains(cadena))
                            nuevaGrama[keyActual].Add(cadena);
                    }
                }
            }

            return nuevaGrama;
        }

        private int findIndex(string a)
        {
            int i;
            i = -1;
            for (int j = 0 ; j < this.simbolosGramaticales.Length ; j++)
            {
                if(this.simbolosGramaticales[j].Equals(a))
                {
                    i = j;
                    break;
                }
            }
            return i;
        }

        #endregion

        #region Tabla De Analisis Sintactico

        private string[,] calculaTablaDeAnalisis()
        {
            string[,] tabla;
            string finalAumentado;
            string produccion;
            finalAumentado = "";
            finalAumentado += this.producciones.First().Key +  "->" + this.simboloInicial + ".";
            tabla = new string[this.conjuntosCanonicos.Count, this.simbolosGramaticales.Length];
            for (int x = 0; x < this.conjuntosCanonicos.Count; x++)
            {
                for (int y = 0; y < this.simbolosGramaticales.Length; y++)
                {
                    tabla[x, y] = "-";//Iniciamos la cadena con  vacios
                }
            }
            int j;
            int k;
            string a;
            for (int i = 0; i < this.conjuntosCanonicos.Count; i++)
            {
                foreach (var conjunto in this.conjuntosCanonicos[i])
                {
                    produccion = "";
                    foreach (string cadena in conjunto.Value)
                    {
                        produccion = conjunto.Key;
                        produccion += "->" + cadena;
                        if (!produccion.Last().Equals('.'))
                        {
                            if(!char.IsUpper(produccion[produccion.IndexOf('.') + 1]))
                            {
                                j = this.findIndex(produccion[produccion.IndexOf('.') + 1].ToString(), this.simbolosGramaticales);
                                a = this.tablaAFD[i][j];
                                j = this.findIndex(produccion[produccion.IndexOf('.') + 1].ToString());
                                if(!tabla[i, j].Contains("d" + a))
                                {
                                    if (tabla[i, j].Equals("-"))
                                    {
                                        tabla[i, j] = "d" + a;
                                    }
                                    else
                                    {
                                        tabla[i, j] += "/d" + a;
                                    }
                                }
                            }
                        }
                        else if(produccion.Last().Equals('.') && !produccion.Equals(finalAumentado))
                        {
                            k = this.encuentraIndiceProduccion(produccion);
                            foreach(string siguiente in this.siguientes[conjunto.Key])
                            {
                                j = findIndex(siguiente);
                                if(!tabla[i, j].Contains("r" + k.ToString()))
                                {
                                    if(tabla[i,j].Equals("-"))
                                    {
                                        tabla[i, j] = "r" + k.ToString();
                                    }
                                    else
                                    {
                                        tabla[i, j] += "/r" + k.ToString();
                                    }
                                }
                            }

                        }
                        else if (produccion.Equals(finalAumentado))
                        {
                            j = this.findIndex("$");
                            tabla[i ,j] = "ac";
                        }
                    }
                }
            }

            for (int i = 0; i < this.conjuntosCanonicos.Count; i++)
            {
                foreach (string A in this.noTerminales)
                {
                    j = this.findIndex(A, this.simbolosGramaticales);
                    if(j != -1)//A
                    {
                        if (!this.tablaAFD[i][j].Equals("Φ"))
                        {
                            a = this.tablaAFD[i][j];
                            j = this.findIndex(A);
                            tabla[i, j] = a;
                        }
                    }
                }
            }
             return tabla;
        }

        private int findIndex(string a, string[] simbolosGramaticales)
        {
            int idx;
            List<string> simbolos;
            idx = -1;
            if(simbolosGramaticales.Contains("$"))
            {
                simbolos = simbolosGramaticales.ToList();
                simbolos.Remove("$");
                simbolosGramaticales = simbolos.ToArray();
            }
            for (int j = 0; j < simbolosGramaticales.Length; j++)
            {
                if (simbolosGramaticales[j].Equals(a))
                {
                    idx = j;
                    break;
                }
            }
            return idx;
        }

        private int encuentraIndiceProduccion(string produccion)
        {
            int indice;
            string Alpha;
            indice = 0;
            produccion = produccion.Replace(".", "");
            foreach (var A in this.produccionesOriginales)
            {
                foreach (string Betha in A.Value)
                {
                    indice++;
                    Alpha = A.Key + "->" + Betha;
                    if (produccion.Equals(Alpha) || Betha == "ε" && produccion.Equals(A.Key + "->"))
                    {
                        return indice;
                    }
                }
            }
            return indice;
        }

        private int encuentraIndiceProduccionPorIndice(int indice)
        {
            foreach (var A in this.produccionesOriginales)
            {
                foreach (string Betha in A.Value)
                {
                    if(indice == 0)
                        return Betha=="ε"?0:Betha.Count();
                    indice--;
                }
            }
            return 0;
        }
               
        private string encuentraIndiceProduccionPorIndiceCabeza(int indice)
        {
            foreach (var A in this.produccionesOriginales)
            {
                foreach (string Betha in A.Value)
                {
                    if (indice == 0)
                        return A.Key;
                    indice--;
                }
            }
            return "";
        }
        #endregion

        #region Evaluacion de Cadenas

        private bool validaCadena(string cadena)
        {
            bool band;
            bool band2;
            band2 = true;
            band = true;
            if (cadena.Any(c => char.IsUpper(c)))
            {
                band = false;
            }
            if (band)
            {
                for (int i = 0; i < cadena.Length; i++)
                {
                    if (!band2)
                    {
                        break;
                    }
                    foreach (var k in this.producciones)
                    {
                        foreach (string v in k.Value)
                        {
                            if (v.Contains(cadena[i]))
                            {
                                band2 = true;
                                break;
                            }
                            else
                            {
                                band2 = false;
                            }
                        }
                        if (band2)
                        {
                            break;
                        }
                    }
                }
            }
            return band && band2;
        }

        public bool evaluarCadena(string cadena, List<int> estados, int i = 0, int tries = 0)
        {
            if (tries == 5)
                return false;

            if (i == 0 && estados.Count == 0)
            {
                cadena += "$";
                estados = new List<int>();
                estados.Add(0);
            }

            for (; i < cadena.Length;)
            {
                string[] celdas = tablaDeAnalisis[estados.Last(), Array.FindIndex(simbolosGramaticales, cad => cad == cadena[i].ToString())].Split('/');
                if (celdas.Count() > 1)
                {
                    foreach (string celda in celdas)
                    {
                        if (celda[0] == 'd')
                        {
                            i++;
                            int nuevoEstado = int.Parse(celda.Substring(1));
                            estados.Add(nuevoEstado);

                            var newDictionary = estados.ToList();
                            if (evaluarCadena(cadena, newDictionary, i, tries + 1))
                                return true;

                            estados.RemoveAt(estados.Count - 1);
                            i--;

                        }
                        else if (celda[0] == 'r')
                        {
                            int numProduccion = int.Parse(celda.Substring(1)) - 1;
                            int longitud = encuentraIndiceProduccionPorIndice(numProduccion);

                            var newDictionary = estados.ToList();
                            for (int j = 0; j < longitud; j++)
                                estados.RemoveAt(estados.Count - 1);

                            string celdaNueva = tablaDeAnalisis[estados.Last(), Array.FindIndex(simbolosGramaticales, cad => cad == encuentraIndiceProduccionPorIndiceCabeza(numProduccion))];
                            int nuevoEstado = int.Parse(celdaNueva);
                            estados.Add(nuevoEstado);

                            if (evaluarCadena(cadena, estados, i, tries + 1))
                                return true;

                            estados = newDictionary;
                        }
                        else if (celda == "ac") return true;
                    }

                    return false;
                }
                else
                {
                    string celda = celdas[0];
                    if (celda == "-")
                        return false;
                    else if (celda[0] == 'd')
                    {
                        i++;
                        int nuevoEstado = int.Parse(celda.Substring(1));
                        estados.Add(nuevoEstado);
                    }
                    else if (celda[0] == 'r')
                    {
                        int numProduccion = int.Parse(celda.Substring(1)) - 1;
                        int longitud = encuentraIndiceProduccionPorIndice(numProduccion);

                        for (int j = 0; j < longitud; j++)
                            estados.RemoveAt(estados.Count - 1);

                        string celdaNueva = tablaDeAnalisis[estados.Last(), Array.FindIndex(simbolosGramaticales, cad => cad == encuentraIndiceProduccionPorIndiceCabeza(numProduccion))];
                        int nuevoEstado = int.Parse(celdaNueva);
                        estados.Add(nuevoEstado);
                    }
                    else if (celda == "ac") return true;
                }
            }

            return true;
        }

        #endregion

    }
}
