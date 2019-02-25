using System;
using System.Reflection;

namespace THNETII.Common.Reflection
{
    /// <summary>
    /// Provides easy and cached access to the default Custom Attributes of a .NET Assembly.
    /// </summary>
    public class AssemblyAttributesAccessor
    {
        private readonly Assembly assembly;

        private AssemblyCopyrightAttribute attribute_Copyright;

        /// <summary>
        /// Gets copyright information.
        /// </summary>
        /// <value>
        /// A string containing the copyright information, or <c>null</c> if none was specified when the assembly was compiled.
        /// </value>
        public string Copyright => GetAssemblyCustomAttribute(ref attribute_Copyright)?.Copyright.NotNullOrWhiteSpace(otherwise: null);

        private AssemblyTrademarkAttribute attribute_Trademark;

        /// <summary>
        /// Gets trademark information.
        /// </summary>
        /// <value>A String containing trademark information, or <c>null</c> if none was specified when the assembly was compiled.</value>
        public string Trademark => GetAssemblyCustomAttribute(ref attribute_Trademark)?.Trademark.NotNullOrWhiteSpace(otherwise: null);

        private AssemblyProductAttribute attribute_Product;

        /// <summary>
        /// Gets product name information.
        /// </summary>
        /// <value>A string containing the product name, or <c>null</c> if none was specified when the assembly was compiled.</value>
        public string Product => GetAssemblyCustomAttribute(ref attribute_Product)?.Product.NotNullOrWhiteSpace(otherwise: null);

        private AssemblyCompanyAttribute attribute_Company;

        /// <summary>
        /// Gets company name information.
        /// </summary>
        /// <value>A string containing the company name, or <c>null</c> if none was specified when the assembly was compiled.</value>
        public string Company => GetAssemblyCustomAttribute(ref attribute_Company)?.Company.NotNullOrWhiteSpace(otherwise: null);

        private AssemblyDescriptionAttribute attribute_Description;

        /// <summary>
        /// Gets assembly description information.
        /// </summary>
        /// <value>A string containing the assembly description, or <c>null</c> if none was specified when the assembly was compiled.</value>
        public string Description => GetAssemblyCustomAttribute(ref attribute_Description)?.Description.NotNullOrWhiteSpace(otherwise: null);

        private AssemblyTitleAttribute attribute_Title;

        /// <summary>
        /// Gets assembly title information.
        /// </summary>
        /// <value>The assembly title, or <c>null</c> if none was specified when the assembly was compiled.</value>
        public string Title => GetAssemblyCustomAttribute(ref attribute_Title)?.Title.NotNullOrWhiteSpace(otherwise: null);

        private AssemblyConfigurationAttribute attribute_Configuration;

        /// <summary>
        /// Gets assembly configuration information.
        /// </summary>
        /// <value>A string containing the assembly configuration information, or <c>null</c> if none was specified when the assembly was compiled.</value>
        public string Configuration => GetAssemblyCustomAttribute(ref attribute_Configuration)?.Configuration.NotNullOrWhiteSpace(otherwise: null);

        private AssemblyDefaultAliasAttribute attribute_DefaultAlias;

        /// <summary>
        /// Gets default alias information.
        /// </summary>
        /// <value>A string containing the default alias information, or <c>null</c> if none was specified when the assembly was compiled.</value>
        public string DefaultAlias => GetAssemblyCustomAttribute(ref attribute_DefaultAlias)?.DefaultAlias.NotNullOrWhiteSpace(otherwise: null);

        private AssemblyInformationalVersionAttribute attribute_InformationalVersion;

        /// <summary>
        /// Gets version information.
        /// </summary>
        /// <value>A string containing the version information, or <c>null</c> if none was specified when the assembly was compiled.</value>
        public string InformationalVersion => GetAssemblyCustomAttribute(ref attribute_InformationalVersion)?.InformationalVersion.NotNullOrWhiteSpace(otherwise: null);

        private AssemblyFileVersionAttribute attribute_FileVersion;

        /// <summary>
        /// Gets the Win32 file version resource name as a string.
        /// </summary>
        /// <value>A string containing the file version resource name.</value>
        public string FileVersionString => GetAssemblyCustomAttribute(ref attribute_FileVersion)?.Version.NotNullOrWhiteSpace(otherwise: null);

        /// <summary>
        /// Gets the Win32 file version resource name.
        /// </summary>
        /// <value>The parsed <see cref="Version"/> from <see cref="FileVersionString"/> if parsing succeeds; otherwise, <c>null</c>.</value>
        public Version FileVersion => Version.TryParse(FileVersionString, out Version fileVersion) ? fileVersion : null;

        private AssemblyCultureAttribute attribute_Culture;

        /// <summary>
        /// Gets the supported culture of the attributed assembly.
        /// </summary>
        /// <value>A string containing the name of the supported culture, or <c>null</c> if none was specified when the assembly was compiled.</value>
        public string Culture => GetAssemblyCustomAttribute(ref attribute_Culture)?.Culture.NotNullOrWhiteSpace(otherwise: null);

        private AssemblyVersionAttribute attribute_Version;

        /// <summary>
        /// Gets the version number of the attributed assembly.
        /// <para>Applications should use the <see cref="AssemblyName.Version"/> member of the <see cref="AssemblyName"/> property instead.</para>
        /// </summary>
        /// <value>A string containing the assembly version number, or <c>null</c> if none was specified when the assembly was compiled.</value>
        /// <remarks>The <see cref="AssemblyName.Version"/> member of the <see cref="AssemblyName"/> property should be used instead.</remarks>
        /// <seealso cref="VersionAttributeValue"/>
        /// <seealso cref="AssemblyName"/>
        /// <seealso cref="AssemblyName.Version"/>
        public string VersionAttributeString => GetAssemblyCustomAttribute(ref attribute_Version)?.Version.NotNullOrWhiteSpace(otherwise: null);

        /// <summary>
        /// Gets the version number of the attributed assembly.
        /// <para>Applications should use the <see cref="AssemblyName.Version"/> member of the <see cref="AssemblyName"/> property instead.</para>
        /// </summary>
        /// <value>The assembly version number, or <c>null</c> if the assembly is not attributed with a version number, or <c>null</c> if the version could be parsed as a <see cref="Version"/> value.</value>
        public Version VersionAttributeValue => Version.TryParse(VersionAttributeString, out Version version) ? version : null;

        private AssemblyKeyFileAttribute attribute_KeyFile;

        /// <summary>
        /// Gets the name of the file containing the key pair used to generate a strong name
        /// for the attributed assembly.
        /// </summary>
        /// <value>A string containing the name of the file that contains the key pair, or <c>null</c> if none was specified when the assembly was compiled.</value>
        public string KeyFile => GetAssemblyCustomAttribute(ref attribute_KeyFile)?.KeyFile.NotNullOrWhiteSpace(otherwise: null);

        private AssemblyDelaySignAttribute attribute_DelaySign;

        /// <summary>
        /// Gets a value indicating whether or not the assembly is not fully signed when created.
        /// </summary>
        /// <value><c>true</c> if the assembly has been built as delay-signed; otherwise, <c>false</c>.</value>
        public bool DelaySign => GetAssemblyCustomAttribute(ref attribute_DelaySign)?.DelaySign ?? false;

        private AssemblySignatureKeyAttribute attribute_SignatureKey;

        /// <summary>
        /// Gets the public key for the strong name used to sign the assembly.
        /// </summary>
        /// <value>The public key for the assembly, or <c>null</c> if the assembly was not signed when it was compiled.</value>
        public string SignaturePublicKey => GetAssemblyCustomAttribute(ref attribute_SignatureKey)?.PublicKey.NotNullOrWhiteSpace(otherwise: null);

        /// <summary>
        /// Gets the countersignature for the strong name for the assembly.
        /// </summary>
        /// <value>The countersignature for the signature key, or <c>null</c> if the assembly was not signed when it was compiled.</value>
        public string CounterSignature => GetAssemblyCustomAttribute(ref attribute_SignatureKey)?.Countersignature.NotNullOrWhiteSpace(otherwise: null);

        private AssemblyKeyNameAttribute attribute_KeyName;

        /// <summary>
        /// Gets the name of the container having the key pair that is used to generate a
        /// strong name for the attributed assembly.
        /// </summary>
        /// <value>A string containing the name of the container that has the relevant key pair, or <c>null</c> if the assembly was not signed when it was compiled.</value>
        public string KeyName => GetAssemblyCustomAttribute(ref attribute_KeyName)?.KeyName.NotNullOrWhiteSpace(otherwise: null);

        private AssemblyName assembly_name;

        /// <summary>
        /// Gets the <see cref="System.Reflection.AssemblyName"/> for the assembly.
        /// </summary>
        public AssemblyName AssemblyName => GetAssemblyName(ref assembly_name);

        /// <summary>
        /// Creates a new Assembly Attributes Accessor instance for the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly from which the attributes are accessed. Must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assembly"/> is <c>null</c>.</exception>
        public AssemblyAttributesAccessor(Assembly assembly)
        {
            this.assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }

        private AssemblyName GetAssemblyName(ref AssemblyName assembly_name_field)
        {
            if (assembly_name_field == null)
                assembly_name_field = assembly.GetName();
            return assembly_name_field;
        }

        private TAttribute GetAssemblyCustomAttribute<TAttribute>(ref TAttribute attribute_field) where TAttribute : Attribute
        {
            if (attribute_field == null)
                attribute_field = assembly.GetCustomAttribute<TAttribute>();
            return attribute_field;
        }
    }
}
