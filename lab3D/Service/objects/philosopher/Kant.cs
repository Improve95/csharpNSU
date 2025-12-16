using Service.service.impl;

namespace Service.objects.philosopher;

public class Kant(string name, ForkFactory forkFactory, Strategy strategy) 
    : Philosopher("Кант", forkFactory, strategy) {}