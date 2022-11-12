using api.Repositories;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace api.Models;

public class Insurance
{
    public int InsuranceId { get; set; }
    public string? Name { get; set; }
    public int Value { get; set; }
    public int? ParentId { get; set; }
    [JsonIgnore]
    public virtual Insurance? Parent { get; set; }
    [JsonIgnore]
    public virtual ICollection<Insurance> Children { get; } = new List<Insurance>();

	//Gets the total combined values of this insurance and all its children, with a specified depth
	public int CombinedValue(int depth)
    {
		if (depth <= 0 || Children.Count <= 0)
		{
			return Value;
		}

		int totalValue = Value;

		foreach (Insurance child in Children)
		{
			totalValue += child.CombinedValue(depth - 1);
		}
		return totalValue;
	}
}
