namespace DataJam.Testing;

using System;
using System.Linq.Expressions;
using System.Reflection;

using JetBrains.Annotations;

/// <summary>A base class for identity strategies.</summary>
/// <typeparam name="T">The element Type.</typeparam>
/// <typeparam name="TIdentity">The identity Type.</typeparam>
[PublicAPI]
public abstract class IdentityStrategy<T, TIdentity> : IIdentityStrategy<T>
    where T : class
{
    private readonly Action<T> _identitySetter;

    private readonly object _lastValueLock = new();

    /// <summary>Initializes a new instance of the <see cref="IdentityStrategy{T, TIdentity}" /> class.</summary>
    /// <param name="propertyExpression">An expression identifying the identity property.</param>
    protected IdentityStrategy(Expression<Func<T, TIdentity>> propertyExpression)
    {
        _identitySetter = obj =>
        {
            var propertyInfo = GetPropertyFromExpression(propertyExpression);
            var id = (TIdentity)propertyInfo.GetValue(obj, null)!;

            if (DefaultValueIsUnset(id))
            {
                propertyInfo.SetValue(obj, Next(), null);
            }
        };
    }

    /// <summary>Gets or sets the function used to set the identity.</summary>
    public Func<TIdentity> Generator { get; protected set; } = null!;

    /// <summary>Gets or sets the last value of the identity for this Type.</summary>
    public TIdentity LastValue { get; protected set; } = default!;

    /// <summary>Assigns the identity values.</summary>
    /// <param name="entity">The entity to assign the identity value for.</param>
    public void Assign(T entity)
    {
        _identitySetter.Invoke(entity);
    }

    /// <summary>Gets the next identity value.</summary>
    /// <returns>The next identity value.</returns>
    /// <exception cref="NotImplementedException">Thrown if the <see cref="Generator" /> is null.</exception>
    public TIdentity Next()
    {
        if (Generator == null)
        {
            throw new NotImplementedException();
        }

        return Generator.Invoke();
    }

    /// <summary>Checks whether the default value for this identity strategy is unset.</summary>
    /// <param name="id">The identity Type.</param>
    /// <returns>A value indicating whether the default value for this identity strategy is unset.</returns>
    protected abstract bool DefaultValueIsUnset(TIdentity id);

    /// <summary>Sets the <see cref="LastValue" /> property to the given <paramref name="value" />.</summary>
    /// <param name="value">The value to use.</param>
    protected void SetLastValue(TIdentity value)
    {
        lock (_lastValueLock)
        {
            LastValue = value;
        }
    }

    private PropertyInfo GetPropertyFromExpression(Expression<Func<T, TIdentity>> propertyExpression)
    {
        MemberExpression memberExpression;

        // this line is necessary, because sometimes the expression
        // comes as Convert(originalExpression)
        if (propertyExpression.Body is UnaryExpression bodyExpression)
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
        else if (propertyExpression.Body is MemberExpression body)
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
