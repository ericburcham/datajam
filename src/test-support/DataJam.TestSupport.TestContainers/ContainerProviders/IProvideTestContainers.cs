// Copyright (c) Enterprise Products Partners L.P. (Enterprise). All rights reserved.
// For copyright details, see the COPYRIGHT file in the root of this repository.
// Licensed under the MIT license. For full license information, see the LICENSE file in the root of this repository.

namespace DataJam.TestSupport.TestContainers;

using System.Collections.Generic;

using DotNet.Testcontainers.Containers;

public interface IProvideTestContainers
{
    IEnumerable<IContainer> TestContainers { get; }
}
