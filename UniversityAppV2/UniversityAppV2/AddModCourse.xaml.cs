using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UniversityAppV2.Models;
using UniversityAppV2.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UniversityAppV2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddModCourse : ContentPage
    {

        public string CourseName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumOfCus { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; } = "";

        //Intructor
        public string InstructorName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }


        public int TermId { get; set; }
        public Term CurrentTerm { get; set; }
        public bool Modify { get; set; }
        public Course CurrentCourse { get; set; }

        public AddModCourse(bool modify, Term t, Course c)
        {
            InitializeComponent();
            BindingContext = this;
            CurrentTerm = t;
            CurrentCourse = c;
            Modify = modify;
            StartDateField.MinimumDate = CurrentTerm.StartDate;
            StartDateField.MaximumDate = CurrentTerm.EndDate;
            EndDateField.MinimumDate = CurrentTerm.StartDate;
            EndDateField.MaximumDate = CurrentTerm.EndDate;

            if (Modify)
                PrefillTxtFields();
            else
                PageTitleLbl.Text = "New Course";
        }

        private void PrefillTxtFields()
        {
            PageTitleLbl.Text = "Modify Course";
            NameField.Text = CurrentCourse.Name;
            StartDateField.Date = CurrentCourse.StartDate;
            EndDateField.Date = CurrentCourse.EndDate;
            NumCusField.SelectedItem = CurrentCourse.NumCus;
            StatusField.SelectedItem = CurrentCourse.Status;
            InstrucNameField.Text = CurrentCourse.InstructorName;
            InstrucPhoneField.Text = CurrentCourse.InstructorPhoneNumber;
            InstrucEmailField.Text = CurrentCourse.InstructorEmail;
            NotesField.Text = CurrentCourse.Notes;

        }

        private void SubmitBtn_Clicked(object sender, EventArgs e)
        {
            if (FormValid())
            {
                if (Modify)
                {
                    CurrentCourse.Name = CourseName;
                    CurrentCourse.StartDate = StartDate;
                    CurrentCourse.EndDate = EndDate;
                    CurrentCourse.NumCus = NumOfCus;
                    CurrentCourse.Status = Status;
                    CurrentCourse.InstructorName = InstructorName;
                    CurrentCourse.InstructorPhoneNumber = Phone;
                    CurrentCourse.InstructorEmail = Email;
                    CurrentCourse.Notes = Notes;

                    TermsDB.UpdateCourse(CurrentCourse);
                }
                else
                {
                    Course newCourse = new Course
                    {
                        Name = CourseName,
                        StartDate = this.StartDate,
                        EndDate = this.EndDate,
                        NumCus = NumOfCus,
                        Status = this.Status,
                        InstructorName = this.InstructorName,
                        InstructorPhoneNumber = Phone,
                        InstructorEmail = Email,
                        Notes = this.Notes,
                        TermId = CurrentTerm.Id
                    };

                    TermsDB.AddCourse(newCourse);
                }


                CurrentTerm.TotalCUs = TermsDB.GetTotalCusForTerm(CurrentTerm.Id);

                TermsDB.UpdateTerm(CurrentTerm);

                Navigation.PopAsync();
            }
        }

        private bool FormValid()
        {
            //for email validation
            Regex email = new Regex("^\\S+@\\S+\\.\\S+$");

            //Input Validations
            if (string.IsNullOrWhiteSpace(CourseName))
            {
                App.Current.MainPage.DisplayAlert("Invalid Course Name", "Please enter a name for the course", "Ok");
                return false;
            }
            else if (StartDateField.Date > EndDateField.Date)
            {
                App.Current.MainPage.DisplayAlert("Invalid Dates", "Start Date is greater than End Date. Time travel is not yet possible", "Ok");
                return false;
            }
            else if(NumOfCus <= 0 || NumOfCus > 6)
            {
                App.Current.MainPage.DisplayAlert("Invalid CUs Selection", "Please pick a number of credits for this course from the provided list(picker)", "Ok");
                return false;
            }
            else if (Status == null)
            {
                App.Current.MainPage.DisplayAlert("Invalid Course Status", "Please select a status for this course", "Ok");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(InstructorName))
            {
                App.Current.MainPage.DisplayAlert("Invalid Instructor Name", "Please enter a name for the instructor of this course", "Ok");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(Phone) || Phone.Length != 10)
            {
                App.Current.MainPage.DisplayAlert("Invalid Phone Number", "The phone number provided is not valid, must be a 10-digit phone number (All numbers). Avoid Country code and any special characters.", "Ok");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(Email))
            {
                App.Current.MainPage.DisplayAlert("Invalid Email", "Please enter the course instructor's email address", "Ok");
                return false;
            }
            else if (!email.IsMatch(Email))
            {
                App.Current.MainPage.DisplayAlert("Invalid Email", "The email provded is not valid, make sure to include the @ symbol and a domain (ex .com)", "Ok");
                return false;
            }

            if (Notes == "")
                Notes = "None provided";

            return true;
        }

    }
}