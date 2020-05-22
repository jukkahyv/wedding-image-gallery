using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeddingImageGallery.Client {
	public class PasswordCheckContext {
		public Action PasswordValidChanged { get; set; }
		public bool PasswordIsValid { get; private set; }
		public void PasswordValid() {
			PasswordIsValid = true;
			PasswordValidChanged();
		}
	}
}
