using System;
using System.Collections.Generic;
using System.Linq;

namespace TiK_KR_Drozdov
{
    class HuffmanTree
    {
        private Node _head;
        public Dictionary<string, string> CodesDictionary;
        
        private double _entropy;
        private double _averageLength;

        public double GetEntropy
        {
            get
            {
                return _entropy;
            }
        }

        public double GetAverageLength
        {
            get
            {
                return _averageLength;
            }
        }

        /// <summary>
        ///     Конструктор построения дерева
        /// </summary>
        /// <param name="inputNodes"> Очередь входных узлов </param>
        public HuffmanTree(string inputString)
        {
            Dictionary<string, double> symbolProbabilities = inputString
                .GroupBy(c => c.ToString().ToLower())
                .ToDictionary(g => g.Key, g => (double)g.Count() / inputString.Length);


            Queue<Node> inputNodes = new Queue<Node>();
            foreach (KeyValuePair<string, double> pair in symbolProbabilities)
            {
                inputNodes.Enqueue(new Node(pair.Value, pair.Key));
            }
            inputNodes = new Queue<Node>(inputNodes.OrderBy(node => node.Frequency)); // Сортировка по возрастанию по частоте


            while (inputNodes.Count != 1) // Построение дерева Хаффмана
            {
                inputNodes.Enqueue(inputNodes.Dequeue() + inputNodes.Dequeue());
                inputNodes = new Queue<Node>(inputNodes.OrderBy(node => node.Frequency));
            }
            _head = inputNodes.Dequeue();

            CodesDictionary = GetCodesDictionary(); // Построение словаря символов и кодов

            CalculateEntropy(symbolProbabilities);
            CalculateAverageLength(symbolProbabilities);
        }

        /// <summary>
        /// Получение словаря кодов для символов обходом дерева в глубину
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetCodesDictionary()
        {
            _head.FindCode();
            CodesDictionary = new Dictionary<string, string>();
            Stack<Node> stack = new Stack<Node>();

            // Обход дерева Хаффмана в глубину
            stack.Push(_head);
            while (stack.Count != 0)
            {
                Node tempNode = stack.Pop();
                if (tempNode.Symbol != null) CodesDictionary.Add(tempNode.Symbol, tempNode.Code);
                do
                {
                    if (tempNode.Left != null)
                    {
                        if (tempNode.Right != null) stack.Push(tempNode.Right);
                        tempNode = tempNode.Left;
                        if (tempNode.Symbol != null) CodesDictionary.Add(tempNode.Symbol, tempNode.Code);
                    }
                    else
                    {
                        tempNode = tempNode.Right;
                        if (tempNode?.Symbol != null)
                            CodesDictionary.Add(tempNode.Symbol, tempNode.Code);
                    }
                } while (tempNode != null);
            }

            // Сортирвка полученного словаря символов и кодов
            CodesDictionary = CodesDictionary.OrderBy(symbol => symbol.Key)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            return CodesDictionary;
        }

        public string Encode(string inputString)
        {
            string huffmanCode = "";
            for (int i = 0; i < inputString.Length; i++)
            {
                CodesDictionary.TryGetValue(inputString[i].ToString(), out string currentCode);
                huffmanCode += currentCode;
            }

            return huffmanCode;
        }


        /// <summary>
        /// Декодирование строки
        /// </summary>
        /// <param name="encodedString"> Входная закодированная строка </param>
        /// <returns> Возвращает декодированную строку в коде Хаффмана </returns>
        public string Decode(string encodedString)
        {
            var decodeString = "";
            var currentNode = _head;

            // Обход дерева в сответствии с входной закодированной строкой
            for (var i = 0; i < encodedString.Length; i++)
            {
                if (encodedString[i] == '0')
                {
                    currentNode = currentNode.Left;
                }
                else
                {
                    currentNode = currentNode.Right;
                }


                if (currentNode != null)
                {
                    if (currentNode.Symbol != null)
                    {
                        decodeString += currentNode.Symbol;
                        currentNode = _head;
                    }
                }
                else
                {
                    return "Ошибка";
                }
            }

            return decodeString;
        }


        private void CalculateEntropy(Dictionary<string, double> symbolProbabilities)
        {
            _entropy = 0;
            foreach (KeyValuePair<string, double> pair in symbolProbabilities)
            {
                _entropy += pair.Value * Math.Log(pair.Value, 2);
            }
            _entropy = -_entropy;
        }

        public void CalculateAverageLength(Dictionary<string, double> symbolProbabilities)
        {
            _averageLength = 0;
            foreach (KeyValuePair<string, double> pair in symbolProbabilities)
            {
                string currentStr = pair.Key;
                CodesDictionary.TryGetValue(currentStr, out string code);
                symbolProbabilities.TryGetValue(currentStr, out double probability);
                _averageLength += code.Length * probability;
            }
        }
    }


    internal class Node : IComparable<Node>
    {
        public string Code; // Код узла
        public string Symbol; // Символ узла
        public double Frequency; // Частота узла

        public Node Left; // Левый потомок
        public Node Right; // Правый потомок
        public Node Parent; // Предок


        public Node(double frequency, string symbol)
        {
            this.Frequency = frequency;
            this.Symbol = symbol;
        }

        private Node(double frequency, Node left, Node right)
        {
            this.Frequency = frequency;
            this.Left = left;
            this.Right = right;
        }


        /// <summary>
        /// Сравнение узлов происходит по частоте узлов
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Node other)
        {
            return Frequency.CompareTo(other);
        }


        /// <summary>
        /// Определение кода узла
        /// </summary>
        /// <param name="code"></param>
        public void FindCode(string code = "")
        {
            if (Symbol != "") this.Code = code;
            Left?.FindCode(code + "0");
            Right?.FindCode(code + "1");
        }

        /// <summary>
        /// Перегрузка оператора сложения узлов
        /// </summary>
        /// <param name="lNode"> Первый узел </param>
        /// <param name="rNode"> Второй узел </param>
        /// <returns> Возвращает новый узел, образованный сложением двух узлов </returns>
        public static Node operator +(Node lNode, Node rNode)
        {
            var newNode = new Node(lNode.Frequency + rNode.Frequency, lNode, rNode);
            lNode.Parent = newNode;
            rNode.Parent = newNode;
            return newNode;
        }
    }

}
