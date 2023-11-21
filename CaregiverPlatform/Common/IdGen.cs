namespace CaregiverPlatform.Common {
    public static class IdGen {
        
        public static int GetId() {
            var random = new Random();
            var randomNum = random.Next(1000, 99999);
            return randomNum + random.Next(1, 100);
        }
    }
}
