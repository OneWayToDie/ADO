using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Academy.Models;

namespace Academy
{
	public partial class StudentForm : HumanForm
	{
		public StudentForm()
		{
			InitializeComponent();

			tbLastName.Text = "Жук";
			tbFirstName.Text = "Василий";
			tbMiddleName.Text = "Петрович";
			dtpBirthDate.Text = "1977.10.24";
			tbEmail.Text = "bazilik_spb@mail.ru";
			tbPhone.Text = "+7(911)024-56-78";

			DataTable groups = DataBase.connector.Select("SELECT * FROM Groups");
			cbGroup.DataSource = groups;
			cbGroup.DisplayMember = "group_name";
			cbGroup.ValueMember = "group_id";
		}
		protected override void buttonOK_Click(object sender, EventArgs e)
		{
			Models.Student student = new Models.Student
				(
				tbLastName.Text,
				tbFirstName.Text,
				tbMiddleName.Text,
				dtpBirthDate.Value.ToString("yyyy-MM-dd"),
				tbEmail.Text,
				tbPhone.Text,
				pbPhoto.Image,
				Convert.ToInt32(cbGroup.SelectedValue)
				);
			DataBase.connector.Insert("Students", $"{student.GetNames()}", $"{student.GetValues()}");
			//DataBase.connector.Insert
			//	(
			//	"Students",
			//	"last_name,first_name,middle_name,birth_date,email,phone,[group]",
			//	$"{tbLastName.Text},{tbFirstName.Text},{tbMiddleName.Text},{dtpBirthDate.Value.ToString("yyyy-MM-dd")}," +
			//	$"{tbEmail.Text}," +
			//	$"{tbPhone.Text}," +
			//	$"{cbGroup.SelectedValue}"
			//	);
		}

	}
}
