using Service.service.impl;

namespace Service.objects.philosopher;

public class Platon(string name, ForkFactory forkFactory, Strategy strategy) 
    : Philosopher("Платон", forkFactory, strategy) {}