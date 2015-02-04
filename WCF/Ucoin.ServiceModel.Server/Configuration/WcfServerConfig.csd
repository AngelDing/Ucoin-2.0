<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="2f2945c6-42dc-4cba-b667-96f2e2d89b6c" namespace="Ucoin.ServiceModel.Server.Configuration" xmlSchemaNamespace="Ucoin.ServiceModel.Server.Configuration" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="WcfServerSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="wcf.server">
      <elementProperties>
        <elementProperty name="Service" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="service" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/2f2945c6-42dc-4cba-b667-96f2e2d89b6c/ServiceElement" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="ServiceElement">
      <attributeProperties>
        <attributeProperty name="Address" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="address" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/2f2945c6-42dc-4cba-b667-96f2e2d89b6c/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/2f2945c6-42dc-4cba-b667-96f2e2d89b6c/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>