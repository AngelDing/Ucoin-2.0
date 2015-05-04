
namespace Ucoin.Framework.Messaging
{
	using System;
	
	public interface ICommand
    {
		/// <summary>
		/// Gets the command identifier.
		/// </summary>
		Guid Id { get; }
    }
}
