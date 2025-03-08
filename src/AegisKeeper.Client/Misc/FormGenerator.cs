using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using MudBlazor;

namespace AegisKeeper.Client.Misc;

public class FormGenerator<TEntity>
 {
     private readonly TEntity _entity;
     private readonly ICollection<PropertyInfo> _properties;
     
     public FormGenerator(TEntity entity)
     {
         _entity = entity;
         ArgumentNullException.ThrowIfNull(entity);
 
         _properties = entity.GetType().GetProperties();
     }
 
     private RenderFragment RenderTextInput(PropertyInfo property)
     {
         return builder =>
         {
             var seq = 0;
             
             builder.OpenComponent<MudTextField<string>>(seq++);
             DefaultAttributes(builder, property, ref seq);
             builder.CloseComponent();
         };
     }
 
     private void DefaultAttributes(RenderTreeBuilder builder, PropertyInfo property, ref int seq)
     {
         builder.AddAttribute(seq++, nameof(MudBaseInput<string>.Label), property.Name);
         builder.AddAttribute(seq++, nameof(MudBaseInput<string>.Variant), Variant.Outlined);

         var instanceExpression = Expression.Constant(_entity);
         
         var propertyAccess = Expression.Property(instanceExpression, property);
         var funcType = typeof(Func<>).MakeGenericType(property.PropertyType);
         var lambdaExpr = Expression.Lambda(funcType, propertyAccess);
         builder.AddAttribute(seq++, nameof(MudBaseInput<string>.For), lambdaExpr);
     }
 }