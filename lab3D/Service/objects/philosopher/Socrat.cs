using Service.service.impl;

namespace Service.objects.philosopher;

public class Socrat(string name, ForkFactory forkFactory, Strategy strategy) 
    : Philosopher("Сократ", forkFactory, strategy) {}