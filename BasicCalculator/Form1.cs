using System;
using System.Linq;
using System.Windows.Forms;

namespace BasicCalculator
{
    /// <summary>
    /// A basic calculator
    /// </summary>
    public partial class Form1 : Form
    {
        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }
        #endregion

        #region Clearing Methods

        private void CEButton_Click(object sender, EventArgs e)
        {
            //Clear the text from the user input text box
            this.UserInputText.Text = string.Empty;

            //Focus the user input text
            FocusInputText();
        }

        private void DelButton_Click(object sender, EventArgs e)
        {
            DeleteTextValue();

            FocusInputText();
        }
        #endregion

        #region Number Methods

        private void SevenButton_Click(object sender, EventArgs e)
        {
            InsertTextValue("7");

            FocusInputText();
        }

        private void EightButton_Click(object sender, EventArgs e)
        {
            InsertTextValue("8");

            FocusInputText();
        }

        private void NineButton_Click(object sender, EventArgs e)
        {
            InsertTextValue("9");

            FocusInputText();
        }

        private void FourButton_Click(object sender, EventArgs e)
        {
            InsertTextValue("4");

            FocusInputText();
        }

        private void FiveButton_Click(object sender, EventArgs e)
        {
            InsertTextValue("5");

            FocusInputText();
        }

        private void SixButton_Click(object sender, EventArgs e)
        {
            InsertTextValue("6");

            FocusInputText();
        }

        private void OneButton_Click(object sender, EventArgs e)
        {
            InsertTextValue("1");

            FocusInputText();
        }

        private void TwoButton_Click(object sender, EventArgs e)
        {
            InsertTextValue("2");

            FocusInputText();
        }

        private void ThreeButton_Click(object sender, EventArgs e)
        {
            InsertTextValue("3");

            FocusInputText();
        }

        private void ZerroButton_Click(object sender, EventArgs e)
        {
            InsertTextValue("0");

            FocusInputText();
        }

        #endregion

        #region Operator Methods
        private void DivideButton_Click(object sender, EventArgs e)
        {
            InsertTextValue("/");

            FocusInputText();
        }

        private void TimesButton_Click(object sender, EventArgs e)
        {
            InsertTextValue("*");

            FocusInputText();
        }

        private void MinusButton_Click(object sender, EventArgs e)
        {
            InsertTextValue("-");

            FocusInputText();
        }

        private void PlusButton_Click(object sender, EventArgs e)
        {
            InsertTextValue("+");

            FocusInputText();
        }

        private void DotButton_Click(object sender, EventArgs e)
        {
            InsertTextValue(".");

            FocusInputText();
        }

        private void EqualsButton_Click(object sender, EventArgs e)
        {
            CalculateEquation();

            FocusInputText();
        }

        #endregion

        /// <summary>
        /// Calculates the given equation and outputs the answer to the user label
        /// </summary>
        private void CalculateEquation()
        {
            

            this.CalculationResultText.Text = ParseOperation();

            FocusInputText();
        }

        /// <summary>
        /// Parses the user equation and calculates the result
        /// </summary>
        /// <returns></returns>
        private string ParseOperation()
        {
            try
            {
                var input = this.UserInputText.Text;

                //Remove all spaces
                input = input.Replace(" ", "");

                //Create a new top-level operation
                var operation = new Operation();
                var leftSide = true;

                for (int i = 0; i < input.Length; i++)
                {
                    //TODO: Handle order priority
                    //       4 + 5 * 3
                    //Check is the current character is a number
                    if("0123456789.".Any(c => input[i] == c))
                    {
                        if (leftSide)
                            operation.LeftSide = AddNumberPart(operation.LeftSide, input[i]); 
                        else
                            operation.RightSide = AddNumberPart(operation.RightSide, input[i]);
                    }
                    //If it is an operator ( + - * / ) set the operator type
                    else if ("+-*/".Any(c => input[i] == c))
                    {
                        //If we are on the right side already, we now need to calculate our current operation
                        //and set the result to the left side of the next operation
                        if (!leftSide)
                        {
                            //Get the operator type
                            var operatorType = GetOperationType(input[i]);

                            //Check if we actually have a left side
                            if (operation.RightSide.Length == 0)
                            {
                                //Check the operator is not minus (as they could be creating a negative number)
                                if (operatorType != OperationType.Minus)
                                    throw new InvalidOperationException($"Operator (+ * / or more than one -) specified without an right side number");

                                //If we got here, the operator type is a minus, and there is no right number currently, so add the minus to the number
                                operation.RightSide += input[i];
                            }
                            else
                            {
                                //Calculate previous equation and set to the left side
                                operation.LeftSide = CalculateOperation(operation);

                                //Set new operator
                                operation.OperationType = operatorType;

                                //Clear the previous right number
                                operation.RightSide = string.Empty;
                            }
                        }
                        else
                        {
                            //Get the operator type
                            var operatorType = GetOperationType(input[i]);

                            //Check if we actually have a left side
                            if (operation.LeftSide.Length == 0)
                            {
                                //Check the operator is not minus (as they could be creating a negative number)
                                if (operatorType != OperationType.Minus)
                                    throw new InvalidOperationException($"Operator (+ * / or more than one -) specified without an left side number");

                                //If we got here, the operator type is a minus, and there is no left number currently, so add the minus to the number
                                operation.LeftSide += input[i];
                            }
                            else
                            {
                                //If we get here, we have a left number and now an operator, so we want to move to the right side

                                //Set the operation type
                                operation.OperationType = operatorType;

                                //Move to the right side
                                leftSide = false;
                            }
                        }
                    }
                }

                //If we are done parsing, and there were no exceptions
                //calculate the current operation
                return CalculateOperation(operation);
            }
            catch(Exception ex)
            {
                return $"Invalid equation. {ex.Message}";
            }
        }

        /// <summary>
        /// Calculates a <see cref="Oparation"/> and returns the result
        /// </summary>
        /// <param name="operation">The operation to calculate</param>
        private string CalculateOperation(Operation operation)
        {
            //Store the number values of the string representations
            decimal left = 0;
            decimal right = 0;

            //Check if we have a valid left side number
            if (string.IsNullOrEmpty(operation.LeftSide) || !decimal.TryParse(operation.LeftSide, out left))
                throw new InvalidOperationException($"Left side of the operation was not a number. {operation.LeftSide}");

            //Check if we have a valid right side number
            if (string.IsNullOrEmpty(operation.RightSide) || !decimal.TryParse(operation.RightSide, out right))
                throw new InvalidOperationException($"Right side of the operation was not a number. {operation.RightSide}");

            try
            {
                switch (operation.OperationType)
                {
                    case OperationType.Add:
                        return (left + right).ToString();
                    case OperationType.Minus:
                        return (left - right).ToString();
                    case OperationType.Divide:
                        return (left / right).ToString();
                    case OperationType.Multiply:
                        return (left * right).ToString();
                    default:
                        throw new InvalidOperationException($"Unknown operator type when calculating operation. {operation.OperationType}");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to calculate operation {operation.LeftSide} {operation.OperationType} {operation.RightSide}. {ex.Message}");
            }
        }

        /// <summary>
        /// Accepts a character and returns the known <see cref="OperationType"/>
        /// </summary>
        /// <param name="character">The character to parse</param>
        /// <returns></returns>
        private OperationType GetOperationType(char character)
        {
            switch (character)
            {
                case '+':

                    return OperationType.Add;
                case '-':

                    return OperationType.Minus;
                case '/':

                    return OperationType.Divide;
                case '*':

                    return OperationType.Multiply;
                default:
                    throw new InvalidOperationException($"Unknown operator type {character}");
            }
        }

        /// <summary>
        /// Attempts to add a new character to the number, checking for a valid character as it goes
        /// </summary>
        /// <param name="currentNumber">The current number string</param>
        /// <param name="newCharacter">The new character to append to the string</param>
        /// <returns></returns>
        private string AddNumberPart(string currentNumber, char newCharacter)
        {
            //Check if there is already a . in the number
            if (newCharacter == '.' && currentNumber.Contains('.'))
                throw new InvalidOperationException($"Number {currentNumber} already contains a . and another cannot be added");

            return currentNumber + newCharacter;
        }

        #region Private Helppers

        private void FocusInputText()
        {
            this.UserInputText.Focus();
        }

        private void InsertTextValue(string value)
        {
            //Remember selection start
            var selectionStart = this.UserInputText.SelectionStart;

            //Set new text
            this.UserInputText.Text = this.UserInputText.Text.Insert(this.UserInputText.SelectionStart, value);

            //Restore selction start
            this.UserInputText.SelectionStart = selectionStart + value.Length;

            //Set selection length to 1
            this.UserInputText.SelectionLength = 0;
        }

        private void DeleteTextValue()
        {
            //If we don"t have a value to delete, return
            if (this.UserInputText.Text.Length < this.UserInputText.SelectionStart + 1)
                return;

            //Remember selection start
            var selectionStart = this.UserInputText.SelectionStart;

            //Delete the character to the right of the selection
            this.UserInputText.Text = this.UserInputText.Text.Remove(this.UserInputText.SelectionStart, 1);

            //Restore selection start
            this.UserInputText.SelectionStart = selectionStart;

            //Set selection length to 1
            this.UserInputText.SelectionLength = 0;
        }

        #endregion
    }
}
