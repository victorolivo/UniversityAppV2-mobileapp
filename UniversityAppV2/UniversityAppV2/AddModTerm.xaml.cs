using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityAppV2.Models;
using UniversityAppV2.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UniversityAppV2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddModTerm : ContentPage
    {
        public string TermTitle { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddMonths(6);
        public int TotalCUs { get; set; }
        public bool Modify { get; set; }
        public Term CurrentTerm { get; set; }

        public AddModTerm(bool modify, Term term)
        {
            InitializeComponent();
            Modify = modify;
            CurrentTerm = term;
            BindingContext = this;

            if (Modify)
                PrefillTxtFields();
            else
                PageTitleLbl.Text = "New Term";
        }

        private void PrefillTxtFields()
        {
            PageTitleLbl.Text = "Modify Term";
            TitleField.Text = CurrentTerm.Title;
            StartDateField.Date = CurrentTerm.StartDate;
            EndDateField.Date = CurrentTerm.EndDate;
        }

        private void SubmitBtn_Clicked(object sender, EventArgs e)
        {
            if (FormValid())
            {
                if (Modify)
                {
                    CurrentTerm.Title = TermTitle;
                    CurrentTerm.StartDate = StartDate;
                    CurrentTerm.EndDate = EndDate;

                    TermsDB.UpdateTerm(CurrentTerm);
                }
                else
                {
                    Term newTerm = new Term
                    {
                        Title = TermTitle,
                        StartDate = this.StartDate,
                        EndDate = this.EndDate,
                        TotalCUs = 0
                    };

                    TermsDB.AddTerm(newTerm);
                }

                Navigation.PushAsync(new MainPage());
            }


        }

        private bool FormValid()
        {
            if (string.IsNullOrWhiteSpace(TitleField.Text))
            {
                App.Current.MainPage.DisplayAlert("Invalid Title", "Please enter a title for the term", "Ok");
                return false;
            }
            else if(StartDateField.Date > EndDateField.Date)
            {
                App.Current.MainPage.DisplayAlert("Invalid Dates", "Start Date is greater than End Date. Time travel is not yet possible", "Ok");
                return false;
            } 

            return true;
        }
    }
}