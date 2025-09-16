// Copyright (c) PlaceholderCompany. All rights reserved.

namespace DataJam.TestSupport.TestPatterns.Family;

public class GetChildren : Query<Child>
{
    public GetChildren()
    {
        this.Selector = dataSource => dataSource.CreateQuery<Child>();
    }
}
