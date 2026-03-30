using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy
{
	internal class ComboItem
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public ComboItem(int id, string name) { Id = id; Name = name; }
		public override string ToString() => Name;
	}
}
