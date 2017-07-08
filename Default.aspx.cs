using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PapaBobsPizza
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void orderButton_Click(object sender, EventArgs e)
        {
            if (nameTextBox.Text.Trim().Length == 0)
            {
                validationLabel.Text = "Please enter a name.";
                validationLabel.Visible = true;
                return;
            }

            if (addressTextBox.Text.Trim().Length == 0)
            {
                validationLabel.Text = "Please enter an address.";
                validationLabel.Visible = true;
                return;
            }

            if (zipTextBox.Text.Trim().Length == 0)
            {
                validationLabel.Text = "Please enter a zip code.";
                validationLabel.Visible = true;
                return;
            }

            if (phoneTextBox.Text.Trim().Length == 0)
            {
                validationLabel.Text = "Please enter a phone number";
                validationLabel.Visible = true;
                return;
            }

            try
            {
                var order = buildOrder();
                PapaBobs.Domain.OrderManager.CreateOrder(order);
                Response.Redirect("success.aspx");
            }
            catch (Exception ex)
            {
                validationLabel.Text = ex.Message;
                validationLabel.Visible = true;
                return;
            }

        }

        //Order Validation
        private PapaBobs.DTO.Enums.PaymentType determinePaymentType()
        {
            PapaBobs.DTO.Enums.PaymentType paymentType;
            if (cashRadioButton.Checked)
            {
                paymentType = PapaBobs.DTO.Enums.PaymentType.Cash;
            }
            else
            {
                paymentType = PapaBobs.DTO.Enums.PaymentType.Credit;
            }

            return paymentType;
        }

        private PapaBobs.DTO.Enums.CrustType determineCrust()
        {
            PapaBobs.DTO.Enums.CrustType crust;
            if (!Enum.TryParse(crustDropDownList.SelectedValue, out crust))
            {
                throw new Exception("Could not determine Pizza crust.");
            }
            return crust;
        }

        private PapaBobs.DTO.Enums.SizeType determineSize()
        {
            PapaBobs.DTO.Enums.SizeType size;
            if (!Enum.TryParse(sizeDropDownList.SelectedValue, out size))
            {
                throw new Exception("Could not deterine Pizza size.");
            }
            return size;
        }

        protected void recalculateTotalCost(object sender, EventArgs e)
        {
            if (sizeDropDownList.SelectedValue == String.Empty) return;
            if (crustDropDownList.SelectedValue == String.Empty) return;
            var order = buildOrder();

            try
            {
                totalLabel.Text = PapaBobs.Domain.PizzaPriceManager.CalculateCost(order).ToString("C");
            }
            catch
            {
                // Swallow the error
            }

        }

        private PapaBobs.DTO.OrderDTO buildOrder()
        {
            var order = new PapaBobs.DTO.OrderDTO();

            //Crust & Size          
            order.Size = determineSize();
            order.Crust = determineCrust();

            //Toppings (values returned based on checked box)
            order.Sausage = sausageCheckBox.Checked;
            order.Pepperoni = pepperoniCheckBox.Checked;
            order.Onions = onionsCheckBox.Checked;
            order.GreenPeppers = greenPeppersCheckBox.Checked;

            //Customers personal info
            order.Name = nameTextBox.Text;
            order.Address = addressTextBox.Text;
            order.Zip = zipTextBox.Text;
            order.Phone = phoneTextBox.Text;
            order.PaymentType = determinePaymentType();

            return order;
        }

    }
}