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
        //Properties for data binding with xaml
        public string TermTitle { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddMonths(6);
        public int TotalCUs { get; set; }
        public bool Modify { get; set; }
        public Term CurrentTerm { get; set; }

        //Modify view according to action: Creating New or Modifying Existing
        public AddModTerm(bool modify, Term term)
        {
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            Modify = modify;
            CurrentTerm = term;
            BindingContext = this;
            StartDateField.MinimumDate = DateTime.Now;

            if (Modify)
                PrefillTxtFields();
            else
                PageTitleLbl.Text = "New Term";
        }

        //Trigger: internal
        //Action: Prefill Fields if in modify mode
        private void PrefillTxtFields()
        {
            PageTitleLbl.Text = "Modify Term";
            TitleField.Text = CurrentTerm.Title;
            StartDateField.Date = CurrentTerm.StartDate;
            EndDateField.Date = CurrentTerm.EndDate;
        }

        //Trigger: User attempts to submit
        //Action: If valid, performs appropiate action according to mode: Creates New or Saves changes made to database
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

        //Trigger: internal
        //Action: Input validation
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

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}