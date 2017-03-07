﻿using System;
using Spark.Engine.Core;
using Spark.Engine.Search;
using Spark.Engine.Service;
using Spark.Engine.Store.Interfaces;
using Spark.Service;
using Spark.Store.Sql.Model;
using Spark.Store.Sql.StoreExtensions;

namespace Spark.Store.Sql
{
    //public class SqlScopedFhirStoreBuilder<T> : IFhirStoreBuilder, IFhirStoreScopeBuilder<T>
    //{
    //    private readonly IFhirDbContext _context;
    //    private readonly Func<T, int> _scopeKeyProvider;

    //    public SqlScopedFhirStoreBuilder(IFhirDbContext context, Func<T, int> scopeKeyProvider)
    //    {
    //        _context = context;
    //        _scopeKeyProvider = scopeKeyProvider;
    //    }

    //    public SqlScopedFhirStore<T> BuildSqlStore(Uri baseUri)
    //    {
    //        SqlScopedFhirStore<T> store = new SqlScopedFhirStore<T>(new FormatId(), _scopeKeyProvider, _context);
    //        store.AddExtension(new SqlScopedSearchFhirStore(
    //            new IndexService(new FhirModel(), new FhirPropertyIndex(new FhirModel()),
    //                new ResourceVisitor(new FhirPropertyIndex(new FhirModel())), new ElementIndexer(new FhirModel()),
    //                new SqlScopedIndexStore()),
    //            new SqlScopedFhirIndex(new FormatId(), _context), new Localhost(baseUri), store ));
    //        store.AddExtension(new SqlScopedHistoryFhirExtension(new SqlScopedSnapshotStore(_context), new Localhost(baseUri), _context));
    //        ((IFhirStore)store).AddExtension(new PagingStore(new SqlScopedSnapshotStore(_context), new Transfer(store, new Localhost(baseUri))));

    //        return store;
    //    }

    //    IFhirStore IFhirStoreBuilder.BuildStore(Uri baseUri)
    //    {
    //        return BuildSqlStore(baseUri);
    //    }

    //    public IScopedFhirStore<T> BuildStore(Uri baseUri)
    //    {
    //        return BuildSqlStore(baseUri);
    //    }
    //}

   
}