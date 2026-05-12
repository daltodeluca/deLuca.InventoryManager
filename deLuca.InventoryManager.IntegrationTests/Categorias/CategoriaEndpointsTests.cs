using System.Net.Http.Headers;
using deLuca.InventoryManager.Application.DTOs;
using deLuca.InventoryManager.Application.Features.Categorias.Commands;
using deLuca.InventoryManager.IntegrationTests.Fixtures;

namespace deLuca.InventoryManager.IntegrationTests.Categorias;

public sealed class CategoriaEndpointsTests(IntegrationTestFixture fixture)
    : IClassFixture<IntegrationTestFixture>, IAsyncLifetime
{
    public async Task InitializeAsync() => await fixture.ResetDatabaseAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    // ── GET /api/categorias ────────────────────────────────────────────────────

    [Fact]
    public async Task GetAll_ReturnsEmptyList_WhenNoCategorias()
    {
        var response = await fixture.Client.GetAsync("/api/categorias");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<IEnumerable<CategoriaResponse>>();
        body.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAll_ReturnsAllActiveCategories_AfterCreation()
    {
        var token = await fixture.GetAdminTokenAsync();
        fixture.Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        await fixture.Client.PostAsJsonAsync("/api/categorias",
            new CreateCategoriaCommand("Cabos", "CAB-"));
        await fixture.Client.PostAsJsonAsync("/api/categorias",
            new CreateCategoriaCommand("Periféricos", "PER-"));

        fixture.Client.DefaultRequestHeaders.Authorization = null;

        var response = await fixture.Client.GetAsync("/api/categorias");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<IEnumerable<CategoriaResponse>>();
        body.Should().HaveCount(2);
    }

    // ── GET /api/categorias/{id} ───────────────────────────────────────────────

    [Fact]
    public async Task GetById_Returns404_WhenNotFound()
    {
        var response = await fixture.Client.GetAsync("/api/categorias/999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetById_ReturnsCategoria_WhenExists()
    {
        var token = await fixture.GetAdminTokenAsync();
        fixture.Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var created = await fixture.Client.PostAsJsonAsync("/api/categorias",
            new CreateCategoriaCommand("Cabos", "CAB-"));
        var categoria = await created.Content.ReadFromJsonAsync<CategoriaResponse>();

        fixture.Client.DefaultRequestHeaders.Authorization = null;

        var response = await fixture.Client.GetAsync($"/api/categorias/{categoria!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<CategoriaResponse>();
        body!.Nome.Should().Be("Cabos");
        body.Prefixo.Should().Be("CAB-");
    }

    // ── POST /api/categorias ───────────────────────────────────────────────────

    [Fact]
    public async Task Create_Returns401_WhenUnauthenticated()
    {
        var response = await fixture.Client.PostAsJsonAsync("/api/categorias",
            new CreateCategoriaCommand("Cabos", "CAB-"));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_Returns201_WithValidData()
    {
        var token = await fixture.GetAdminTokenAsync();
        fixture.Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await fixture.Client.PostAsJsonAsync("/api/categorias",
            new CreateCategoriaCommand("Equipamentos", "EQP-"));

        fixture.Client.DefaultRequestHeaders.Authorization = null;

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<CategoriaResponse>();
        body!.Id.Should().BePositive();
        body.Nome.Should().Be("Equipamentos");
        body.Prefixo.Should().Be("EQP-");
    }

    [Fact]
    public async Task Create_Returns422_WhenPrefixoDoesNotHaveFourChars()
    {
        var token = await fixture.GetAdminTokenAsync();
        fixture.Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await fixture.Client.PostAsJsonAsync("/api/categorias",
            new CreateCategoriaCommand("Cabos", "CB"));

        fixture.Client.DefaultRequestHeaders.Authorization = null;

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task Create_Returns422_WhenNomeIsEmpty()
    {
        var token = await fixture.GetAdminTokenAsync();
        fixture.Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await fixture.Client.PostAsJsonAsync("/api/categorias",
            new CreateCategoriaCommand("", "CAB-"));

        fixture.Client.DefaultRequestHeaders.Authorization = null;

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    // ── PUT /api/categorias/{id} ───────────────────────────────────────────────

    [Fact]
    public async Task Update_Returns204_WhenSuccessful()
    {
        var token = await fixture.GetAdminTokenAsync();
        fixture.Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var created = await fixture.Client.PostAsJsonAsync("/api/categorias",
            new CreateCategoriaCommand("Cabos", "CAB-"));
        var categoria = await created.Content.ReadFromJsonAsync<CategoriaResponse>();

        var updateResponse = await fixture.Client.PutAsJsonAsync(
            $"/api/categorias/{categoria!.Id}",
            new UpdateCategoriaCommand(0, "Cabos Atualizados", "CABU"));

        fixture.Client.DefaultRequestHeaders.Authorization = null;

        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await fixture.Client.GetAsync($"/api/categorias/{categoria.Id}");
        var updated = await getResponse.Content.ReadFromJsonAsync<CategoriaResponse>();
        updated!.Nome.Should().Be("Cabos Atualizados");
        updated.Prefixo.Should().Be("CABU");
    }

    [Fact]
    public async Task Update_Returns404_WhenNotFound()
    {
        var token = await fixture.GetAdminTokenAsync();
        fixture.Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await fixture.Client.PutAsJsonAsync("/api/categorias/999",
            new UpdateCategoriaCommand(0, "Nome", "NOM-"));

        fixture.Client.DefaultRequestHeaders.Authorization = null;

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── DELETE /api/categorias/{id} ────────────────────────────────────────────

    [Fact]
    public async Task Delete_Returns204_AndHidesFromGetAll()
    {
        var token = await fixture.GetAdminTokenAsync();
        fixture.Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var created = await fixture.Client.PostAsJsonAsync("/api/categorias",
            new CreateCategoriaCommand("Temporário", "TEMP"));
        var categoria = await created.Content.ReadFromJsonAsync<CategoriaResponse>();

        var deleteResponse = await fixture.Client.DeleteAsync($"/api/categorias/{categoria!.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        fixture.Client.DefaultRequestHeaders.Authorization = null;

        var getAll = await fixture.Client.GetAsync("/api/categorias");
        var list = await getAll.Content.ReadFromJsonAsync<IEnumerable<CategoriaResponse>>();
        list.Should().NotContain(c => c.Id == categoria.Id);
    }

    [Fact]
    public async Task Delete_Returns404_WhenNotFound()
    {
        var token = await fixture.GetAdminTokenAsync();
        fixture.Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await fixture.Client.DeleteAsync("/api/categorias/999");

        fixture.Client.DefaultRequestHeaders.Authorization = null;

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
