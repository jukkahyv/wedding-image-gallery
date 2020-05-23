using System;

namespace WeddingImageGallery.Client {
	public class PasswordCheckContext {
		public Action PasswordValidChanged { get; set; }
		public bool PasswordIsValid { get; private set; }
		public void PasswordValid() {
			PasswordIsValid = true;
			PasswordValidChanged?.Invoke();
		}
	}
}
