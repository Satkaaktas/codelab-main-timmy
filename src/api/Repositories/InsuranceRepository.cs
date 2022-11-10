using System;
using System.Collections.Generic;
using System.Linq;
using api.Data;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class InsuranceRepository : IRepository<Insurance>, IDisposable
{
    private InsuranceDbContext context;

    public InsuranceRepository(InsuranceDbContext context)
    {
        this.context = context;
    }

    public IEnumerable<int> GetTop(int maxCount, int maxDepth)
    {
        List<Insurance> insurances = context.Insurances.ToList();
        List<int> topValues = new List<int>(maxCount);

        //Get only top level insurances
        insurances = insurances.Where(i => i.ParentId == null || i.ParentId == 0).ToList();
        
        foreach(Insurance insurance in insurances)
        {
            topValues.Add(GetCombinedValue(insurance, maxDepth));
        }

        //Placing them in descending order
        topValues.Sort();
        topValues.Reverse();

        //Removing everything except the top X insurances
        for (int i = topValues.Count-1; i >= maxCount ; i--)
        {
            topValues.RemoveAt(i);
        }

        return topValues;
    }

    //Gets the total combined values of an insurance and all it's childred, with a specified depth
    private int GetCombinedValue(Insurance insurance, int depth)
    {
        if(depth <= 0 || insurance.Children.Count <= 0)
        {
            return insurance.Value;
        }
        int totalValue = insurance.Value;
        foreach(Insurance child in insurance.Children)
        {
			totalValue += GetCombinedValue(child, depth - 1);
		}
        return totalValue;
    }
        


    public IEnumerable<Insurance> GetAll()
    {
        return context.Insurances.ToList();
    }

    public Insurance? GetById(int insuranceId)
    {
        return context.Insurances.Find(insuranceId);
    }

    public void Insert(Insurance insurance)
    {
        context.Insurances.Add(insurance);
    }

    public void Delete(int insuranceId)
    {
        Insurance? insurance = context.Insurances.Find(insuranceId);
        if (insurance != null)
            context.Insurances.Remove(insurance);
    }

    public void Update(Insurance insurance)
    {
        context.Entry(insurance).State = EntityState.Modified;
    }

    public void Save()
    {
        context.SaveChanges();
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}