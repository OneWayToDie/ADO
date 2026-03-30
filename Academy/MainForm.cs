using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace Academy
{
	public partial class MainForm : Form
	{
		Query[] queries =
		{
			new Query
				(
				"last_name,first_name,middle_name,group_name,direction_name",
				"Students,Groups,Directions",
				"[group]=group_id AND direction=direction_id"
				),
			new Query
				(
				"*",
				"Groups,Directions",
				"direction=direction_id"
				),
			new Query("*","Directions"),
			new Query("*","Disciplines"),
			new Query("*","Teachers"),
		};
		string[] status_messages =
			{
			"Количество студентов",
			"Количество групп",
			"Количество направлений",
			"Количество дисциплин",
			"Количество преподавателей"
		};
		DataGridView[] tables;
		DBtools.Connector connector;


		public MainForm()
		{
			InitializeComponent();
			tables = new DataGridView[] { dgvStudents, dgvGroups, dgvDirections, dgvDisciplines, dgvTeachers };
			connector = new DBtools.Connector(ConfigurationManager.ConnectionStrings["PV_521_Import"].ConnectionString);
			LoadDirections();
			LoadGroupsForStudent();

			tabControl_SelectedIndexChanged(tabControl, null);
		}

		private void LoadDirections()
		{
			DataTable directions = connector.Select("SELECT direction_id, direction_name FROM Directions");
			FillCombo(cmbStudentsDirection, directions);
			FillCombo(cmbGroupsDirection, directions);
			FillCombo(cmbDisciplinesDirection, directions);
		}

		private void FillCombo(ComboBox cmb, DataTable data)
		{
			cmb.Items.Clear();
			cmb.Items.Add(new ComboItem(0, "Все направления"));
			foreach (DataRow row in data.Rows)
				cmb.Items.Add(new ComboItem(Convert.ToInt32(row["direction_id"]), row["direction_name"].ToString()));
			cmb.SelectedIndex = 0;
		}
		private void LoadGroupsForStudent()
		{
			int directionId = GetSelectedId(cmbStudentsDirection);
			string filter = directionId == 0 ? "" : $"WHERE direction_id = {directionId}";
			DataTable groups = connector.Select($"SELECT group_id, group_name FROM Groups{filter}");
			cmbStudentsGroups.Items.Clear();
			cmbStudentsGroups.Items.Add(new ComboItem(0, "Все группы"));
			foreach (DataRow row in groups.Rows)
				cmbStudentsGroups.SelectedIndex = 0;
		}

		private void UpdateStudents()
		{
			int directionId = GetSelectedId(cmbStudentsDirection);
			int groupId = GetSelectedId(cmbStudentsGroups);
			string where = "";
			if (groupId != 0) where = $" AND [group] = {groupId}";
			else if (directionId != 0) where = $" AND direction_id = {directionId}";
			string query = $@"
							SELECT last_name, first_name, middle_name, group_name, direction_name
							FROM Students, Groups, Directions
							WHERE [group] = group_id AND direction_id = direction_id{where}";
			SetTable(0, query);
		}
		private void UpdateDisciplines()
		{

		}
		private void UpdateGroups()
		{
			int directionId = GetSelectedId(cmbGroupsDirection);
			string where = directionId == 0 ? "" : $" AND direction_id = {directionId}";
			string query = $@"
							SELECT group_id, group_name, direction_name
							FROM Groups, Directions
							WHERE direction_id = direction_id{where}";
			SetTable(1, query);
		}
		private void SetTable(int index, string query)
		{
			tables[index].DataSource = connector.Select(query);
			toolStripStatusLabel.Text = $"{status_messages[index]}: {tables[index].RowCount}";
		}
		private int GetSelectedId(ComboBox cmb) => (cmb.SelectedItem as ComboItem)?.Id ?? 0;
		private void cmbStudentsDirection_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadGroupsForStudent();
			if (tabControl.SelectedIndex == 0) UpdateStudents();
		}

		private void cmbStudentsGroups_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabControl.SelectedIndex == 0) UpdateStudents();
		}

		private void cmbGroupsDirection_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabControl.SelectedIndex == 1) UpdateGroups();
		}

		private void cmbDisciplinesDirection_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabControl.SelectedIndex == 3) UpdateDisciplines();
		}
		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = tabControl.SelectedIndex;
			tables[i].DataSource = connector.Select(queries[i].ToString());
			toolStripStatusLabel.Text = $"{status_messages[i]}: {tables[i].RowCount - 1}";
		}
	}
}
