namespace BFF.Fruits;

public record Fruit;

public class BananaException : Exception
{
}

public class KiwiException : Exception
{
}

public interface IFruitRepository
{
    Fruit FindById(int i);
}

public class FruitRepository : IFruitRepository
{
    public Fruit FindById(int i)
    {
        if (i == 2)
        {
            throw new BananaException();
        }

        return new Fruit();
    }
}



public class FruitService
{
    private readonly IFruitRepository _fruitRepository;

    public FruitService(IFruitRepository fruitRepository)
    {
        _fruitRepository = fruitRepository;
    }

    public Fruit GetFruitById(int id)
    {
        try
        {
            return _fruitRepository.FindById(id);
        }
        catch (BananaException e)
        {
            Console.WriteLine(e);
            throw new FruitNotFoundException();
        }   
    }
}

public class FruitNotFoundException : Exception
{
}