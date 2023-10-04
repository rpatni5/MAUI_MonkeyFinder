namespace GeneratedNamespace {
    
    
    public class DynamicModel {
        
        public @string type;
        
        public Properties properties;
        
        public class Properties {
            
            public Number Number;
            
            public RequestedDeviceType RequestedDeviceType;
            
            public DeliveryMethod DeliveryMethod;
            
            public Customer Customer;
            
            public Vehicle Vehicle;
            
            public class Number {
                
                public @string type;
            }
            
            public class RequestedDeviceType {
                
                public @string type;
            }
            
            public class DeliveryMethod {
                
                public @string type;
            }
            
            public class Customer {
                
                public @string type;
                
                public Properties properties;
                
                public class Properties {
                    
                    public Contacts Contacts;
                    
                    public Name Name;
                    
                    public Number Number;
                    
                    public OverrideData OverrideData;
                    
                    public class Contacts {
                        
                        public @string type;
                        
                        public Properties properties;
                        
                        public class Properties {
                            
                            public FirstName FirstName;
                            
                            public Name Name;
                            
                            public Email Email;
                            
                            public City City;
                            
                            public Address Address;
                            
                            public MobilePhone MobilePhone;
                            
                            public class FirstName {
                                
                                public @string type;
                            }
                            
                            public class Name {
                                
                                public @string type;
                            }
                            
                            public class Email {
                                
                                public @string type;
                            }
                            
                            public class City {
                                
                                public @string type;
                            }
                            
                            public class Address {
                                
                                public @string type;
                            }
                            
                            public class MobilePhone {
                                
                                public @string type;
                            }
                        }
                    }
                    
                    public class Name {
                        
                        public @string type;
                    }
                    
                    public class Number {
                        
                        public @string type;
                    }
                    
                    public class OverrideData {
                        
                        public @string type;
                    }
                }
            }
            
            public class Vehicle {
                
                public @string type;
                
                public Properties properties;
                
                public class Properties {
                    
                    public VIN VIN;
                    
                    public MakeModelCode MakeModelCode;
                    
                    public LicensePlate LicensePlate;
                    
                    public Make Make;
                    
                    public Model Model;
                    
                    public YearOfInitialRegistration YearOfInitialRegistration;
                    
                    public MotorType MotorType;
                    
                    public OverrideData OverrideData;
                    
                    public class VIN {
                        
                        public @string type;
                    }
                    
                    public class MakeModelCode {
                        
                        public @string type;
                    }
                    
                    public class LicensePlate {
                        
                        public @string type;
                    }
                    
                    public class Make {
                        
                        public @string type;
                    }
                    
                    public class Model {
                        
                        public @string type;
                    }
                    
                    public class YearOfInitialRegistration {
                        
                        public @string type;
                    }
                    
                    public class MotorType {
                        
                        public @string type;
                    }
                    
                    public class OverrideData {
                        
                        public @string type;
                    }
                }
            }
        }
    }
}
