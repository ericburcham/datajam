// Copyright (c) Enterprise Products Partners L.P. (Enterprise). All rights reserved.
// For copyright details, see the COPYRIGHT file in the root of this repository.
// Licensed under the MIT license. For full license information, see the LICENSE file in the root of this repository.

namespace DataJam.TestSupport.TestContainers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DotNet.Testcontainers.Containers;

using NUnit.Framework;

public abstract class TestContainerSetUpFixture<TContainerProvider> : IAsyncDisposable
    where TContainerProvider : IProvideTestContainers
{
    private readonly IEnumerable<IContainer> _containers;

    protected TestContainerSetUpFixture(IEnumerable<TContainerProvider> containerProviders)
        : this(containerProviders.ToArray())
    {
    }

    protected TestContainerSetUpFixture(params TContainerProvider[] containerProviders)
    {
        _containers = containerProviders.SelectMany(x => x.TestContainers);
    }

    public async ValueTask DisposeAsync()
    {
        await Parallel.ForEachAsync(
            _containers,
            async (container, _) =>
            {
                await container.DisposeAsync();
            });

        GC.SuppressFinalize(this);
    }

    [OneTimeTearDown]
    public virtual async Task RunAfterAllTests()
    {
        await Parallel.ForEachAsync(
            _containers,
            async (container, token) =>
            {
                await container.StopAsync(token);
            });
    }

    [OneTimeSetUp]
    public virtual async Task RunBeforeAllTests()
    {
        await Parallel.ForEachAsync(
            _containers,
            async (container, token) =>
            {
                await container.StartAsync(token);
            });
    }
}
