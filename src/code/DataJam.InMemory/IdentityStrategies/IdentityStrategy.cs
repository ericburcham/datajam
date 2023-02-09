using System.Linq.Expressions;
using System.Reflection;

namespace DataJam.InMemory.IdentityStrategies;

public abstract class IdentityStrategy<TType, TIdentity> : IIdentityStrategy<TType>
    where TType : class
{
    private readonly Action<TType> _identitySetter;

    private readonly object _lastValueLock = new object();

    protected IdentityStrategy(Expression<Func<TType, TIdentity>> property)
    {
        _identitySetter = obj =>
        {
            var propertyInfo = GetPropertyFromExpression(property);
            var id = (TIdentity)propertyInfo.GetValue(obj, null);
            if (IsDefaultUnsetValue(id))
            {
                propertyInfo.SetValue(obj, Next(), null);
            }
        };
    }

    public Func<TIdentity> Generator { get; protected set; }

    public TIdentity LastValue { get; protected set; }

    public void Assign(TType entity)
    {
        _identitySetter.Invoke(entity);
    }

    public TIdentity Next()
    {
        if (Generator == null)
        {
            throw new NotImplementedException();
        }

        return Generator.Invoke();
    }

    protected abstract bool IsDefaultUnsetValue(TIdentity id);

    protected void SetLastValue(TIdentity value)
    {
        lock (_lastValueLock)
        {
            LastValue = value;
        }
    }

    private PropertyInfo GetPropertyFromExpression(Expression<Func<TType, TIdentity>> lambda)
    {
        MemberExpression memberExpression;

        // this line is necessary, because sometimes the expression 
        // comes as Convert(originalExpression)
        if (lambda.Body is UnaryExpression bodyExpression)
        {
            if (bodyExpression.Operand is MemberExpression operand)
            {
                memberExpression = operand;
            }
            else
            {
                throw new ArgumentException();
            }
        }
        else if (lambda.Body is MemberExpression body)
        {
            memberExpression = body;
        }
        else
        {
            throw new ArgumentException();
        }

        return (PropertyInfo)memberExpression.Member;
    }
}