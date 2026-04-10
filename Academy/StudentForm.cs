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
		internal Models.Student student;
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
		public StudentForm(int id):this()
		{
			DataTable data = DataBase.connector.Select("*", "Students", $"stud_id={id}");
			//object[] arr = data.Rows[0].ItemArray;
			student = new Models.Student(data.Rows[0].ItemArray);
			human = student;
			Extract();
			cbGroup.SelectedValue = student.group;
			pbPhoto.Image = DataBase.connector.Download_photo("Students", "photo", student.id);
		}
		protected override void buttonOK_Click(object sender, EventArgs e)
		{
			base.buttonOK_Click(sender, e);

			student = new Models.Student(human,Convert.ToInt32(cbGroup.SelectedValue));
			//object id = (int)DataBase.connector.Scalar($"SELECT stud_id FROM Students WHERE {student.GetCondition()}");
			if (student.id == 0) DataBase.connector.Insert("Students", $"{student.GetNames()}", $"{student.GetValues()}");
			else DataBase.connector.Update($"UPDATE Students SET {student.GetUpdateString()} WHERE stud_id={student.id}");
			if(student.photo != null)
				DataBase.connector.Upload_photo(student.SerializePhoto(), student.id, "photo", "Students");

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
