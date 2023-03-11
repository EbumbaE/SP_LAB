namespace Lab3 {
    struct Rule {
        public State State;
        public string NextStateValue;
        public Rule(string state, string elem, string nextStateValue) {
            State = new State(state, elem);
            NextStateValue = nextStateValue;
        }
    }

    struct State {
        public string StateValue, Element;
        public State(string state, string elem) {
            StateValue = state;
            Element = elem;
        }
        public bool CompareTo(State s) {
            return StateValue == s.StateValue && Element == s.Element;
        }
    }

    struct StateMachine {
        public List<string> SetOfStates, Alphabet, FinalStates;
        public string S0, Info;
        public List<Rule> Rules;
        public State State;

        public StateMachine(List<string> setOfStates, List<string> alphabet, List<string> finalStates, string s0, string info) {
            SetOfStates = setOfStates;
            Alphabet = alphabet;
            FinalStates = finalStates;
            S0 = s0;
            Rules = new List<Rule>();
            Info = info;
            State = new State(S0, "");
        }

        public void AddRule(string from, string state, string to) {
            Rules.Add(new Rule(from, state, to));
        }

        public int GetExpressionNumberOfMatchingCharacters(string expression) {
            string elem;
            int amount = 0;
            State.Element = ""; State.StateValue = S0;
            
            for (int i = 0; i < expression.Length; i++) {
                elem = expression[i].ToString();
                State.Element = elem;
                State = NextState(State);
                if (CheckExit(State)) {
                    break;
                }
                amount++;
            }

            return amount;
        }

        private bool CheckExit(State state) {
            for (int i = 0; i < FinalStates.Count; i++) {
                if (state.StateValue == FinalStates[i]) {
                    return true;
                }
            }
            
            return false;
        }

        private State NextState(State state) {
            for(int i = 0; i < Rules.Count; i++) {
                if (state.CompareTo(Rules[i].State)) {
                    return new State(Rules[i].NextStateValue, state.Element); 
                }
            }

            return new State(FinalStates[0], state.Element); 
        }
    }

    class Program {
        const int AmountStateMachines = 2;
        
        static void Main(string[] args) {
                StateMachine[] automate = new StateMachine[] {
                    new StateMachine(
                        new List<string>() { "S01", "A", "qf1" },
                        new List<string>() { " " },
                        new List<string>() { "qf1" },
                        "S01",
                        "\"\\s+\" (for ex.: so   so)"
                    ),
                    new StateMachine(
                        new List<string>() { "S02", "$", "qf2" },
                        new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", 
                        "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", 
                        "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"  },
                        new List<string>() { "qf2" },
                        "S02",
                        "\"^[A-Z][a-zAZ]*\" (for ex.: FfA)"
                    )
                };

            
            automate[0].AddRule("S01", " ", "A");
            automate[0].AddRule("A", " ", "A");
            automate[0].AddRule("A", "", "qf1");


            for(char state = 'A'; state <= 'Z'; state++) {
                automate[1].AddRule("S02", state.ToString(), "$");
            }
            for(char state = 'a'; state <= 'z'; state++) {
                automate[1].AddRule("$", state.ToString(), "$");
            } 
            automate[1].AddRule("$", "A", "$");
            automate[1].AddRule("$", "Z", "$");
            automate[1].AddRule("$", "", "qf2");

            for(int i = 0; i < AmountStateMachines; i++) {
                Console.WriteLine($"Automate {i}: {automate[i].Info}");
            }

            int automateNumber, iteration;
            string expression = "";
            while (true) {
                Console.WriteLine("Enter number of automate or \"-1\" to exit:");
                automateNumber = Convert.ToInt32(Console.ReadLine());
                if (automateNumber == -1) {
                    return;
                }
                Console.WriteLine($"You chose automate number {automateNumber}");

                while (true) {
                    Console.WriteLine($"Enter line {automate[automateNumber].Info} to execute or \"exit\" to change automate number:");
                    if ((expression = Console.ReadLine()) == null) {
                        Console.WriteLine("Error: null readline");
                        return;
                    }
                    if (expression == "exit") {
                        break;
                    }

                    Console.WriteLine($"Length: {expression.Length}");
                    iteration = automate[automateNumber].GetExpressionNumberOfMatchingCharacters(expression);
                    Console.WriteLine($"Iteration: {iteration}");
                    Console.WriteLine($"End State: {automate[automateNumber].State.StateValue}"); 
                }
            }
        }
    }
}