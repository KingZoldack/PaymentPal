using System.Globalization;

namespace PaymentPal
{
    public partial class MainPage : ContentPage
    {
        decimal _bill;
        int _tip;
        int _people = 1;

        public MainPage()
        {
            InitializeComponent();
        }

        private void entEnterbill_Completed(object sender, EventArgs e)
        {
            if (decimal.TryParse(entEnterbill.Text, out decimal bill))
            {
                _bill = bill;
                CalculateTotal();
            }
            else
                DisplayAlert("Error", "Please enter a valid amount", "OK");
        }

        private void CalculateTotal()
        {
            //Total of tip
            var totalTip = (_bill * _tip) / 100;

            //Tip per person
            var tipPerPerson = totalTip / _people;
            var tipPerPersonText = tipPerPerson.ToString("C", new CultureInfo("en-GB"));
            lblTipAmount.Text = tipPerPersonText;

            //Subtotal
            var subTotal = _bill / _people;
            var subTotalText = subTotal.ToString("C", new CultureInfo("en-GB"));
            lblSubtotalAmount.Text = subTotalText;

            //Total
            var totalPerPerson = (_bill + totalTip) / _people;
            var totalPerPersonText = totalPerPerson.ToString("C", new CultureInfo("en-GB"));
            lblTotal.Text = totalPerPersonText;
        }

        private void sldrTipPercent_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            _tip = (int)sldrTipPercent.Value;
            UpdateSliderValue();
        }

        private void TipButton_Clicked(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button button = (Button)sender;
                string buttonText = button.Text.Trim('%');

                if (int.TryParse(buttonText, out int tip))
                {
                    _tip = tip;
                    UpdateSliderValue();
                }
            }
        }

        private void UpdateSliderValue()
        {
            lblTipPercent.Text = $"Tip: {_tip}%";
            sldrTipPercent.Value = _tip;
            CalculateTotal();
        }

        private void ChangePeople(int change)
        {
            if (int.TryParse(entPeopleNumber.Text, out _people))
            {
                _people += change;

                //Clamps poeple to 1 if people < 1
                _people = Math.Max(1, _people);

                entPeopleNumber.Text = _people.ToString();

                CalculateTotal();
            }
            else
                DisplayAlert("Error", "Please enter a whole or valid number", "OK");
        }

        private void btnMinus_Clicked(object sender, EventArgs e) => ChangePeople(-1);
        private void entPeopleNumber_Completed(object sender, EventArgs e) => ChangePeople(0);
        private void btnPlus_Clicked(object sender, EventArgs e) => ChangePeople(1);
    }
}
