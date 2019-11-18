using Microsoft.Extensions.DependencyInjection;

namespace THNETII.DependencyInjection.Nesting
{
    /// <summary>
    /// Specifies the contract for a collection of service descriptors that
    /// are nested beneath a parent service collection.
    /// </summary>
    public interface INestedServiceCollection : IServiceCollection
    {
        /// <summary>
        /// Gets a service collection of all service descriptors that are
        /// inheited from the parent service collection.
        /// </summary>
        /// <value>An <see cref="IServiceCollection"/> containing the services inherited from the parent.</value>
        IServiceCollection? InheritedServices { get; }

        /// <summary>
        /// Gets or sets the behaviour that controls if and how inherited
        /// services are added to the service resolution order of the nested
        /// service container.
        /// </summary>
        /// <value>
        /// A value from the <see cref="THNETII.DependencyInjection.Nesting.RootServicesAddBehavior"/> enumeration,
        /// specifying if and how services added to the nested service collection are
        /// propagated to the parent collection.
        /// </value>
        RootServicesAddBehavior RootServicesAddBehavior { get; set; }
    }

    /// <summary>
    /// Specifies the contract for a collection of service descriptors that
    /// are nested beneath a parent service collection and which are logically
    /// sepeated from sibling containers by a unique Key.
    /// </summary>
    /// <typeparam name="TKey">
    /// The type of the key that distingueshes the nested service collection
    /// from its siblings.
    /// </typeparam>
    public interface INestedServiceCollection<out TKey> : INestedServiceCollection
    {
        /// <summary>
        /// Gets the unique key of the nested service collection.
        /// </summary>
        /// <value>The key of type <typeparamref name="TKey"/> for this nested service collection.</value>
        /// <remarks>Direct siblings should have unique keys.</remarks>
        TKey Key { get; }
    }

    /// <summary>
    /// Specifies the contract for a family of service descriptors that
    /// are nested beneath a parent service collection and which are logically
    /// sepeated from sibling containers by a unique Key.
    /// </summary>
    /// <typeparam name="TFamily">The type of the family of service descriptors.</typeparam>
    /// <typeparam name="TKey">
    /// The type of the key that distingueshes the nested service collection
    /// from its siblings.
    /// </typeparam>
    public interface INestedServiceCollection<TFamily, out TKey>
        : INestedServiceCollection<TKey>
    { }
}
