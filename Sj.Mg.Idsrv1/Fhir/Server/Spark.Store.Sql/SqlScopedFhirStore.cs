﻿using System;
using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Spark.Engine.Core;
using Spark.Store.Sql.Model;
using Resource = Spark.Store.Sql.Model.Resource;
using System.Data.Entity;
using Spark.Core;
using Spark.Engine.Store;

namespace Spark.Store.Sql
{
    //public class SqlScopedFhirStore<T> : IScopedFhirStore<T>, IGenerator
    //{
    //    private readonly IFhirDbContext context;
    //    private readonly IFormatId formatId;
    //    private readonly Func<T, int> scopeKeyProvider;

    //    public IResourceStore ResourceStore { get; set; }
    //    public SqlScopedFhirStore(IFormatId formatId, Func<T, int> scopeKeyProvider, IFhirDbContext dbContext, IResourceStore resourceStore = null)
    //    {
    //        this.context = dbContext;
    //        this.ResourceStore = resourceStore;
    //        this.formatId = formatId;
    //        this.scopeKeyProvider = scopeKeyProvider;
    //    }

    //    protected override void InternalAdd(Entry entry)
    //    {
    //        Resource resource = GetResource(entry);
    //        ResourceContent resourceContent = GetResourceContent(entry);
    //        resourceContent.Resource = resource;

    //        ResourceStore.UpdateResource(entry.Resource, resource);
    //        ResourceStore.UpdateResourceContent(entry.Resource, resourceContent);

    //        context.ResourceVersions.Add(resourceContent);
    //        context.SaveChanges();
    //    }

    //    private ResourceContent GetResourceContent(Entry entry)
    //    {
    //        ResourceContent resourceContent = ResourceStore.CreatResourceContent(entry.Resource);
    //        FillResourceContent(entry, resourceContent);
    //        return resourceContent;
    //    }

    //    private void FillResourceContent(Entry entry, ResourceContent resourceContent)
    //    {
    //        resourceContent.Content = entry.Resource != null
    //            ? FhirSerializer.SerializeResourceToXml(entry.Resource)
    //            : null;
    //        resourceContent.InternalVersionId = formatId.ParseVersionId(entry.Key.VersionId);
    //        resourceContent.Method = entry.Method.ToString();
    //        resourceContent.CreationDate = DateTime.Now.ToUniversalTime();
    //    }

    //    private Resource GetResource(Entry entry)
    //    {
    //        Resource resource;
    //        if (entry.Method == Bundle.HTTPVerb.POST)
    //        {
    //            resource = ResourceStore.CreatResource(entry.Resource);
    //            FillResource(entry, resource);
    //        }
    //        else
    //        {
    //            resource = GetResource(entry.Key);
    //        }
    //        return resource;
    //    }

    //    private Resource FillResource(Entry entry, Resource resource)
    //    {
    //        resource.ResourceType = entry.Key.TypeName;
    //        resource.ResourceId = formatId.ParseResourceId(entry.Key.ResourceId);
    //        resource.CreationDate = DateTime.Now.ToUniversalTime();
    //        resource.Endpoint = entry.Key.WithoutBase().WithoutVersion().ToString();
    //        resource.ScopeKey = scopeKeyProvider(Scope);
    //        return resource;
    //    }
    //    private Resource GetResource(IKey key)
    //    {
    //        int resourceId = formatId.ParseResourceId(key.ResourceId);

    //        Resource resource = RestrictToScope(context.Resources)
    //           .SingleOrDefault(r => r.ResourceType == key.TypeName && r.ResourceId == resourceId);

    //        return resource;
    //    }

    //    public override Entry Get(IKey key)
    //    {
    //        Resource resource = GetResource(key);
    //        if (resource != null)
    //        {
    //            if (key.HasVersionId())
    //            {
    //                int versionId = formatId.ParseVersionId(key.VersionId);
    //                context.Entry(resource)
    //                    .Collection(r => r.ResourceVersions)
    //                    .Query()
    //                    .Where(rv => rv.InternalVersionId == versionId)
    //                    .Load();
    //            }
    //            else
    //            {
    //                context.Entry(resource)
    //                    .Collection(r => r.ResourceVersions)
    //                    .Query()
    //                    .OrderByDescending(rv => rv.InternalVersionId)
    //                    .Take(1)
    //                    .Load();
    //            }
    //            return ParseEntry(resource.ResourceVersions.OrderByDescending(rv => rv.InternalVersionId).First());
    //        }

    //        return null;
    //    }

    //    private IQueryable<Resource> RestrictToScope(IQueryable<Resource> queryable)
    //    {
    //        if (Scope != null)
    //        {
    //            int scopeKey = scopeKeyProvider(Scope);
    //            return queryable.Where(r => r.ScopeKey == scopeKey);
    //        }
    //        return queryable;
    //    }

    //    public override IList<Entry> Get(IEnumerable<string> localIdentifiers, string sortby)
    //    {
    //        List<string> keys = localIdentifiers.Select(l => Key.ParseOperationPath(l))
    //            .Select(k=> k.WithoutVersion().WithoutBase().ToString()).ToList();
    //        List<Resource> resources =
    //             RestrictToScope(context.Resources.Include(r => r.ResourceVersions))
    //               .Where(r=> keys.Contains(r.Endpoint)).ToList();

    //        return resources.Select(r=>ParseEntry(r.ResourceVersions.First())).ToList();
    //    }

    //    private Entry ParseEntry(ResourceContent resourceContent)
    //    {
    //        Entry entry = null;

    //        if (resourceContent != null)
    //        {
    //            entry = Entry.Create((Bundle.HTTPVerb)Enum.Parse(typeof(Bundle.HTTPVerb), resourceContent.Method),
    //                new Key()
    //                {
    //                    TypeName = resourceContent.Resource.ResourceType,
    //                    ResourceId = formatId.GetResourceId(resourceContent.Resource.ResourceId),
    //                    VersionId = formatId.GetVersionId(resourceContent.InternalVersionId)
    //                },
    //                resourceContent.CreationDate);
    //            entry.Resource = resourceContent.Content != null? FhirParser.ParseResourceFromXml(resourceContent.Content) : null;
    //        }
    //        return entry;
    //    }

    //    public string NextResourceId(string resource)
    //    {
    //        int id = RestrictToScope(context.Resources).Where(r=> r.ResourceType == resource).Select(r => r.ResourceId).DefaultIfEmpty(0).Max();
    //        return formatId.GetResourceId(id + 1);
    //    }

    //    public string NextVersionId(string resourceIdentifier)
    //    {
    //        throw new NotSupportedException("this operation is not supported");
    //    }

    //    public bool CustomResourceIdAllowed(string value)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public string NextVersionId(string resourceType, string resourceIdentifier)
    //    {
    //        int scopeKey = scopeKeyProvider(Scope);
    //        string keyPath = Key.Create(resourceType, resourceIdentifier).ToString();
    //        ResourceContent currentResourceContent = context.ResourceVersions.Where(rv => rv.Resource.Endpoint == keyPath &&
    //                                                                        rv.Resource.ScopeKey == scopeKey)
    //            .OrderByDescending(rv => rv.InternalVersionId).Take(1).SingleOrDefault();

    //        int id = currentResourceContent != null
    //            ? currentResourceContent.InternalVersionId
    //            : 0;
    //        return formatId.GetResourceId(id + 1);
    //    }

    //    protected override void SetScopeOnExtension<TV>(TV extension)
    //    {
    //        IScopedFhirExtension<IScope> scopedFhirExtension = extension as IScopedFhirExtension<IScope>;
    //        if (scopedFhirExtension != null)
    //        {
    //            scopedFhirExtension.Scope = new ScopeProvider<T>(scopeKeyProvider, scope);
    //        }
    //    }
    //}
}