using Service.service.impl;

namespace Service.objects.philosopher;

public class Dekart(string name, ForkFactory forkFactory, Strategy strategy) 
    : Philosopher("Декарт",  forkFactory, strategy) {}